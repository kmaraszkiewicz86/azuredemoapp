using AzureJsonDataFlowFunction.Extensions;
using AzureJsonDataFlowFunction.Models.Dto;
using AzureJsonDataFlowFunction.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureJsonDataFlowFunction.Functions
{
    public class SendJsonDataToBlobStorage
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<SendJsonDataToBlobStorage> _logger;

        public SendJsonDataToBlobStorage(IAzureBlobService azureBlobService, ILogger<SendJsonDataToBlobStorage> logger)
        {
            _azureBlobService = azureBlobService;
            _logger = logger;
        }

        [Function("SendJsonDataToBlobStorage")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "jsonfiles")] HttpRequestData req)
        {
            Result result = await _azureBlobService.SendJsonDataAsync(req);

            return await req.ToHttpResponseDataAsync(result);
        }
    }
}