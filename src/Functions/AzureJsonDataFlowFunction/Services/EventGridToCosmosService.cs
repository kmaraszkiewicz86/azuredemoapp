using Azure.Messaging.EventGrid;
using Azure.Storage.Blobs;
using AzureJsonDataFlowFunction.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace AzureJsonDataFlowFunction.Services
{
    public class EventGridToCosmosService : IEventGridToCosmosService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly CosmosClient _cosmosClient;
        private readonly ILogger<EventGridToCosmosService> _logger;
        private const string CosmosDbDatabase = "JsonDb";
        private const string CosmosDbContainer = "JsonFiles";

        public EventGridToCosmosService(
            BlobServiceClient blobServiceClient,
            CosmosClient cosmosClient,
            ILogger<EventGridToCosmosService> logger)
        {
            _blobServiceClient = blobServiceClient;
            _cosmosClient = cosmosClient;
            _logger = logger;
        }

        public async Task ProcessEventAsync(EventGridEvent eventGridEvent)
        {
            _logger.LogInformation("Received Event Grid event: {Id}", eventGridEvent.Id);

            // Parse event data (assumes BlobCreated event)
            BlobCreatedEventData? eventData = eventGridEvent.Data.ToObjectFromJson<BlobCreatedEventData>();

            if (eventData is null)
            {
                _logger.LogError("Event data is null or not of expected type BlobCreatedEventData.");
                return;
            }

            // Parse blob URL to get container and blob name
            Uri blobUri = new(eventData.Url);
            string[] segments = blobUri.AbsolutePath.TrimStart('/').Split('/', 2);
            if (segments.Length < 2)
            {
                _logger.LogError("Invalid blob URL: {Url}", eventData.Url);
                return;
            }

            string containerName = segments[0];
            string blobName = segments[1];

            // Download blob content
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            string blobContent;
            using (MemoryStream downloadStream = new())
            {
                await blobClient.DownloadToAsync(downloadStream);
                downloadStream.Position = 0;
                using StreamReader reader = new(downloadStream);
                blobContent = await reader.ReadToEndAsync();
            }

            // Save event info and blob content to Cosmos DB
            Container cosmosContainer = _cosmosClient.GetContainer(CosmosDbDatabase, CosmosDbContainer);
            BlobEventGridToCosmosItem cosmosItem = new()
            {
                Id = eventGridEvent.Id,
                EventType = eventGridEvent.EventType,
                EventTime = eventGridEvent.EventTime,
                BlobUrl = eventData.Url,
                ContentType = eventData.ContentType,
                Category = "json",
                BlobContent = blobContent
            };

            _logger.LogInformation("Saving event data to Cosmos DB: {Id}", eventGridEvent.Id);
            await cosmosContainer.CreateItemAsync(cosmosItem, new PartitionKey(eventGridEvent.EventType));
        }
    }
}