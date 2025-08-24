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
            string? requestBody = await req.ReadAsStringAsync();

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            _logger.LogInformation($"Get request body: {requestBody ?? "empty"}.");

            if (string.IsNullOrWhiteSpace(requestBody))
            {
                _logger.LogError("Request body cannot be empty.");
                return Result.BadRequest("Request body cannot be empty.");
            }

            if (!await CheckBlobAccountAvailableAsync(req))
            {
                _logger.LogError("Azure Blob Storage account is not available or credentials are invalid.");
                return Result.InternalServerError("Azure Blob Storage account is not available or credentials are invalid.");
            }

            var data = JsonSerializer.Deserialize<SendJsonModels>(requestBody);

            _logger.LogInformation("Connect and try to upload the blob item.");
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

            await containerClient.CreateIfNotExistsAsync();

            var blobName = $"{Guid.NewGuid()}.json";

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            return await UploadBlobItem(requestBody, data, blobClient);
        }

        private async Task<Result> UploadBlobItem(string requestBody, SendJsonModels? data, BlobClient blobClient)
        {
            try
            {
                using (MemoryStream stream = new(System.Text.Encoding.UTF8.GetBytes(requestBody)))
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                // Check if the blob was created successfully
                bool exists = await blobClient.ExistsAsync();
                if (!exists)
                {
                    _logger.LogError("Blob was not created after upload attempt.");
                    return Result.InternalServerError("Blob was not created.");
                }
                else
                {
                    _logger.LogInformation($"Blob uploaded successfully. Blob URI: {blobClient.Uri}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload blob.");
                return Result.InternalServerError("Failed to upload blob.");
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
