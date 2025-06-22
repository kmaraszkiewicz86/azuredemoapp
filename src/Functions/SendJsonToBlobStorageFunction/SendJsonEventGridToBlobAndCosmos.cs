using Azure.Messaging.EventGrid;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BlobEventGridToBlobAndCosmosFunction
{
    public class BlobEventGridToBlobAndCosmos
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly CosmosClient _cosmosClient;
        private readonly ILogger _logger;
        private const string CosmosDbDatabase = "JsonDb";
        private const string CosmosDbContainer = "JsonFiles";

        public BlobEventGridToBlobAndCosmos(
            BlobServiceClient blobServiceClient,
            CosmosClient cosmosClient,
            ILogger<BlobEventGridToBlobAndCosmos> logger)
        {
            _blobServiceClient = blobServiceClient;
            _cosmosClient = cosmosClient;
            _logger = logger;
        }

        [Function("BlobEventGridToBlobAndCosmos")]
        public async Task Run(EventGridEvent eventGridEvent)
        {
            _logger.LogInformation("Received Event Grid event: {Id}", eventGridEvent.Id);

            // Parse event data (assumes BlobCreated event)
            var eventData = eventGridEvent.Data.ToObjectFromJson<BlobCreatedEventData>();

            // Parse blob URL to get container and blob name
            var blobUri = new Uri(eventData.Url);
            // URL format: https://<account>.blob.core.windows.net/<container>/<blob>
            var segments = blobUri.AbsolutePath.TrimStart('/').Split('/', 2);
            if (segments.Length < 2)
            {
                _logger.LogError("Invalid blob URL: {Url}", eventData.Url);
                return;
            }
            var containerName = segments[0];
            var blobName = segments[1];

            // Download blob content
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            string blobContent;
            using (var downloadStream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(downloadStream);
                downloadStream.Position = 0;
                using (var reader = new StreamReader(downloadStream))
                {
                    blobContent = await reader.ReadToEndAsync();
                }
            }

            // Save event info and blob content to Cosmos DB
            var cosmosContainer = _cosmosClient.GetContainer(CosmosDbDatabase, CosmosDbContainer);
            var cosmosItem = new
            {
                id = eventGridEvent.Id,
                eventType = eventGridEvent.EventType,
                eventTime = eventGridEvent.EventTime,
                blobUrl = eventData.Url,
                contentType = eventData.ContentType,
                category = "json",
                blobContent // the actual JSON from the uploaded blob
            };
            await cosmosContainer.CreateItemAsync(cosmosItem, new PartitionKey(eventGridEvent.EventType));
        }

        // Model for BlobCreated event data
        public class BlobCreatedEventData
        {
            public string Url { get; set; }
            public string ContentType { get; set; }
        }
    }
}