using Azure.Messaging.EventGrid;
using Azure.Storage.Blobs;
using AzureJsonDataFlowFunction.Functions;
using AzureJsonDataFlowFunction.Models;
using AzureJsonDataFlowFunction.Models.Dto;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AzureJsonDataFlowFunction.Services
{
    public class EventGridToCosmosService(BlobServiceClient _blobServiceClient,
        CosmosClient _cosmosClient,
        ILogger<BlobEventGridToBlobAndCosmos> _logger) : IEventGridToCosmosService
    {
        private const string CosmosDbDatabase = "JsonDb";
        private const string CosmosDbContainer = "JsonFiles";

        public async Task<Result> ProcessEventAsync(EventGridEvent eventGridEvent)
        {
            _logger.LogInformation("Received Event Grid event: {Id}", eventGridEvent.Id);
            _logger.LogInformation("Data to process: {EventType}", JsonSerializer.Serialize(eventGridEvent));

            // Parse event data (assumes BlobCreated event)
            BlobCreatedEventData? eventData = eventGridEvent.Data.ToObjectFromJson<BlobCreatedEventData>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (eventData is null)
            {
                _logger.LogError("Event data is null or not of expected type BlobCreatedEventData.");
                return Result.InternalServerError("Event data is null or not of expected type BlobCreatedEventData.");
            }

            _logger.LogInformation("Parse blob URL ({Url}) to get container and blob name.", eventData?.Url ?? "Invalid url");

            // Parse blob URL to get container and blob name
            Uri blobUri = new(eventData!.Url);
            string[] segments = blobUri.AbsolutePath.TrimStart('/').Split('/', 2);
            if (segments.Length < 2)
            {
                _logger.LogError("Invalid blob URL: {Url}", eventData.Url);
                return Result.InternalServerError($"Invalid blob URL: {eventData.Url}");
            }

            string containerName = segments[0];
            string blobName = segments[1];

            // Download blob content
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);


            _logger.LogInformation("Downloading the blob content");
            var blobContent = string.Empty;
            using (MemoryStream downloadStream = new())
            {
                await blobClient.DownloadToAsync(downloadStream);
                downloadStream.Position = 0;
                using StreamReader reader = new(downloadStream);
                blobContent = await reader.ReadToEndAsync();
            }

            _logger.LogInformation($"Downloaded the blob content: {blobContent}");
            JsonModel? sendJsonModels = JsonSerializer.Deserialize<JsonModel>(blobContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (sendJsonModels is null)
            {
                return Result.InternalServerError("Deserialized blob content is null or invalid.");
            }

            // Save event info and blob content to Cosmos DB
            Container cosmosContainer = _cosmosClient.GetContainer(CosmosDbDatabase, CosmosDbContainer);
            BlobEventGridToCosmosItem cosmosItem = new()
            {
                id = eventGridEvent.Id,
                EventType = eventGridEvent.EventType,
                EventTime = eventGridEvent.EventTime,
                BlobUrl = eventData.Url,
                ContentType = eventData.ContentType,
                Category = "json",
                JsonModel = sendJsonModels!
            };

            _logger.LogInformation("Saving event data to Cosmos DB: {Id}", eventGridEvent.Id);
            await cosmosContainer.CreateItemAsync(cosmosItem, new PartitionKey(eventGridEvent.EventType));

            return Result.Ok("Event processed and data saved to Cosmos DB successfully.");
        }
    }
}