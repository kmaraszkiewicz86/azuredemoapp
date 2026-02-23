using AzureJsonDataFlowFunction.Models;
using AzureJsonDataFlowFunction.Models.Dto;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureJsonDataFlowFunction.Services
{
    public interface ICosmosDbService : IService
    {
        Task<ResultWithValue<List<JsonModel>>> GetDataAsync(HttpRequestData req);
    }
}