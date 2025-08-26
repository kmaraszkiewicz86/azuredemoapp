using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using AzureJsonDataFlowFunction.Models.Dto;

namespace AzureJsonDataFlowFunction.Extensions
{
    public static class HttpResponseDataExtension
    {
        /// <summary>
        /// Converts a Result object to HttpResponseData.
        /// </summary>
        public static async Task<HttpResponseData> ToHttpResponseDataAsync<TResultParam>(this HttpRequestData req, ResultWithValue<TResultParam> result)
        {
            var response = req.CreateResponse(result.StatusCode);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(result.Value));
            return response;
        }

        /// <summary>
        /// Converts a Result object to HttpResponseData.
        /// </summary>
        public static async Task<HttpResponseData> ToHttpResponseDataAsync(this HttpRequestData req, Result result)
        {
            var response = req.CreateResponse(result.StatusCode);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(new { message = result.Message }));
            return response;
        }
    }
}
