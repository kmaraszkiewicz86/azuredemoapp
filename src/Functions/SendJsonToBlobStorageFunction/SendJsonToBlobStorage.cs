using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace SendJsonToBlobStorageFunction
{
    /// <summary>
    /// Send a json file to Azure Blob Storage.
    /// </summary>
    /// <param name="blobServiceClient"></param>
    /// <param name="logger"></param>
    public partial class SendJsonToBlobStorage(BlobServiceClient blobServiceClient, 
        ILogger<SendJsonToBlobStorage> logger)
    {
        private const string ContainerName = "jsonfiles";

        /// <summary>
        /// Run the function to send JSON to Azure Blob Storage.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Function("SendJsonToBlobStorage")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {

            logger.LogInformation("C# HTTP trigger function processed a request.");
            
            var requestBody = await req.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return new BadRequestObjectResult("Request body cannot be empty.");
            }

            var data = JsonSerializer.Deserialize<DemoPayload>(requestBody);

            var containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

            await containerClient.CreateIfNotExistsAsync();

            var blobName = $"{Guid.NewGuid()}.json";

            var blobClient = containerClient.GetBlobClient(blobName);

            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody)))
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            var name = data?.Name ?? "unknown";

            return new OkObjectResult($"Received JSON for {name}. Everything is OK.!");
        }
    }
}
