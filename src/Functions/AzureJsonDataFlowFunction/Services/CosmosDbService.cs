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

        public async Task<ResultWithValue<List<SendJsonModels>>> GetDataAsync(HttpRequestData req)
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

                //todo: it will be convert later
                SendJsonModels test = null;//results.Select(r => r.BlobContent).ToList();

                return ResultWithValue<List<SendJsonModels>>.Ok(results);
            }
            catch (CosmosException ex)
            {
                return ResultWithValue<List<SendJsonModels>>.InternalServerError(ex.Message);
            }
        }
    }
}
