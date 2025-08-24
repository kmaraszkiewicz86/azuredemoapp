using AzureJsonDataFlowFunction.Models;
using AzureJsonDataFlowFunction.Models.Dto;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureJsonDataFlowFunction.Services
{
    public class CosmosDbService(CosmosClient _cosmosClient) : ICosmosDbService
    {
        private const string CosmosDbDatabase = "JsonDb";
        private const string CosmosDbContainer = "JsonFiles";

        public async Task<ResultWithValue<List<JsonModel>>> GetDataAsync(HttpRequestData req)
        {
            try
            {
                Container container = _cosmosClient.GetContainer(CosmosDbDatabase, CosmosDbContainer);

                var query = "SELECT * FROM c";
                var iterator = container.GetItemQueryIterator<BlobEventGridToCosmosItem>(query);
                List<BlobEventGridToCosmosItem> results = new ();

                while (iterator.HasMoreResults)
                {
                    foreach (var item in await iterator.ReadNextAsync())
                    {
                        results.Add(item);
                    }
                }

                List<JsonModel> jsonModels = results.Select(r => r.JsonModel).ToList();

                return ResultWithValue<List<JsonModel>>.Ok(jsonModels);
            }
            catch (CosmosException ex)
            {
                return ResultWithValue<List<JsonModel>>.InternalServerError(ex.Message);
            }
        }
    }
}
