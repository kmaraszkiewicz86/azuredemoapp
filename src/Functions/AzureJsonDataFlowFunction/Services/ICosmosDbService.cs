using Microsoft.Azure.Functions.Worker.Http;

namespace AzureJsonDataFlowFunction.Services
{
    public interface ICosmosDbService : IService
    {
        Task<List<dynamic>> GetDataAsync(HttpRequestData req);
    }
}