using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;

namespace AzureJsonDataFlowFunction.Extensions
{
    public static class HttpResponseDataExtension
    {
        public static async Task<HttpResponseData> GetResultAsync<TParamType>(this HttpRequestData req, TParamType result, HttpStatusCode statusCode)
        {
            var response = req.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(result));
            return response;
        }
    }
}
