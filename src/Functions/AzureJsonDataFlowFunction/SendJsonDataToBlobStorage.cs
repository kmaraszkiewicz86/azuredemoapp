using Azure.Storage.Blobs;
using AzureJsonDataFlowFunction.Extensions;
using AzureJsonDataFlowFunction.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace AzureJsonDataFlowFunction
{
    public class SendJsonDataToBlobStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<SendJsonDataToBlobStorage> _logger;
        private const string ContainerName = "jsonfiles";

        public SendJsonDataToBlobStorage(BlobServiceClient blobServiceClient, ILogger<SendJsonDataToBlobStorage> logger)
        {
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        [Function("SendJsonDataToBlobStorage")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "jsonfiles/upload")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string? requestBody = await req.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return await req.GetResultAsync("Request body cannot be empty.", HttpStatusCode.BadRequest);
            }

            var data = JsonSerializer.Deserialize<DemoPayload>(requestBody);

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

            await containerClient.CreateIfNotExistsAsync();

            var blobName = $"{Guid.NewGuid()}.json";

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using (MemoryStream stream = new (System.Text.Encoding.UTF8.GetBytes(requestBody)))
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            var name = data?.Name ?? "unknown";

            return await req.GetResultAsync($"Received JSON for {name}. Everything is OK.!", HttpStatusCode.OK);
        }
    }
}