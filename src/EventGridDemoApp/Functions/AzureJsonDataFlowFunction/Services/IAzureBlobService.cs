using AzureJsonDataFlowFunction.Models.Dto;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureJsonDataFlowFunction.Services
{
    public interface IAzureBlobService : IService
    {
        Task<Result> SendJsonDataAsync(HttpRequestData req);
    }
}