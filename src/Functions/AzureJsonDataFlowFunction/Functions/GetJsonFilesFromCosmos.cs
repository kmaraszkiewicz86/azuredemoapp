using AzureJsonDataFlowFunction.Extensions;
using AzureJsonDataFlowFunction.Models;
using AzureJsonDataFlowFunction.Models.Dto;
using AzureJsonDataFlowFunction.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AzureJsonDataFlowFunction.Functions
{

    /// <summary>
    /// Retrieves JSON files from a Cosmos DB container and returns them in an HTTP
    /// response.
    /// </summary>
    /// <remarks>This function queries the Cosmos DB container specified by the constants <see
    /// cref="CosmosDbDatabase"/> and <see cref="CosmosDbContainer"/>. It retrieves all items in the container using a
    /// SQL query and returns them as a JSON array in the HTTP response.</remarks>
    public class GetJsonFilesFromCosmos
    {
        private readonly ICosmosDbService _cosmosService;
        private readonly ILogger<GetJsonFilesFromCosmos> _logger;

        public GetJsonFilesFromCosmos(ICosmosDbService cosmosService, ILogger<GetJsonFilesFromCosmos> logger)
        {
            _cosmosService = cosmosService;
            _logger = logger;
        }


        /// <summary>
        /// Handles HTTP GET requests to retrieve JSON files from a Cosmos DB container.
        /// </summary>
        /// <remarks>This function queries all items in the specified Cosmos DB container using the query
        /// "SELECT * FROM c". The results are serialized into a JSON array and returned in the HTTP response.</remarks>
        /// <param name="req">The HTTP request data, including headers and query parameters.</param>
        /// <returns>An HTTP response containing a JSON array of items retrieved from the Cosmos DB container. The response has a
        /// status code of <see cref="HttpStatusCode.OK"/> and a content type of "application/json".</returns>
        [Function("GetJsonFilesFromCosmos")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "jsonfiles")] HttpRequestData req)
        {
            ResultWithValue<List<SendJsonModels>> results = await _cosmosService.GetDataAsync(req);

            return await req.GetResultAsync(results, HttpStatusCode.BadRequest);
        }
    }
}