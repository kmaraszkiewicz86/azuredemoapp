using Azure.Storage.Blobs;
using AzureJsonDataFlowFunction.Extensions;
using AzureJsonDataFlowFunction.Functions;
using AzureJsonDataFlowFunction.Models;
using AzureJsonDataFlowFunction.Models.Dto;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace AzureJsonDataFlowFunction.Services
{
    public class AzureBlobService(
        BlobServiceClient _blobServiceClient,
        ILogger<SendJsonDataToBlobStorage> _logger) : IAzureBlobService
    {
        private const string ContainerName = "jsonfiles";

        public async Task<Result> SendJsonDataAsync(HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string? requestBody = await req.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return Result.BadRequest("Request body cannot be empty.");
            }

            if (!await CheckBlobAccountAvailableAsync(req))
            {
                return Result.InternalServerError("Azure Blob Storage account is not available or credentials are invalid.");
            }

            var data = JsonSerializer.Deserialize<DemoPayload>(requestBody);

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

            await containerClient.CreateIfNotExistsAsync();

            var blobName = $"{Guid.NewGuid()}.json";

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using (MemoryStream stream = new(System.Text.Encoding.UTF8.GetBytes(requestBody)))
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            var name = data?.Name ?? "unknown";

            return Result.Ok($"Received JSON for {name}. Everything is OK.!");
        }

        private async Task<bool> CheckBlobAccountAvailableAsync(HttpRequestData req)
        {
            try
            {
                await _blobServiceClient.GetPropertiesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Azure Blob Storage account is not available or credentials are invalid.");
                return false;
            }
        }
    }
}
