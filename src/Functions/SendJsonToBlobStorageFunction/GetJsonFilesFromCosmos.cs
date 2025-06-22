using System.Net;
using System.Text.Json;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace BlobEventGridToBlobAndCosmosFunction
{
    public class GetJsonFilesFromCosmos
    {
        private readonly CosmosClient _cosmosClient;
        private readonly ILogger _logger;
        private const string CosmosDbDatabase = "JsonDb";
        private const string CosmosDbContainer = "JsonFiles";

        public GetJsonFilesFromCosmos(
            CosmosClient cosmosClient,
            ILogger<GetJsonFilesFromCosmos> logger)
        {
            _cosmosClient = cosmosClient;
            _logger = logger;
        }

        [Function("GetJsonFilesFromCosmos")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "jsonfiles")] HttpRequestData req)
        {
            var container = _cosmosClient.GetContainer(CosmosDbDatabase, CosmosDbContainer);

            var query = "SELECT * FROM c";
            var iterator = container.GetItemQueryIterator<dynamic>(query);
            var results = new List<dynamic>();

            while (iterator.HasMoreResults)
            {
                foreach (var item in await iterator.ReadNextAsync())
                {
                    results.Add(item);
                }
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(results));
            return response;
        }
    }
}