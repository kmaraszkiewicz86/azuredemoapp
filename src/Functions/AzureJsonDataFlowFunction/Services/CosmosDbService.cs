using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureJsonDataFlowFunction.Services
{
    public class CosmosDbService(CosmosClient _cosmosClient) : ICosmosDbService
    {
        private const string CosmosDbDatabase = "JsonDb";
        private const string CosmosDbContainer = "JsonFiles";

        public async Task<List<dynamic>> GetDataAsync(HttpRequestData req)
        {
            Container container = _cosmosClient.GetContainer(CosmosDbDatabase, CosmosDbContainer);

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

            return results;
        }
    }
}
