using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using AzureJsonDataFlowFunction.Models.Dto;

namespace AzureJsonDataFlowFunction.Extensions
{
    public static class HttpResponseDataExtension
    {
        /// <summary>
        /// Creates an HTTP response with the specified status code and a JSON-serialized body.
        /// </summary>
        /// <remarks>The response includes a "Content-Type" header set to "application/json".</remarks>
        /// <typeparam name="TParamType">The type of the object to serialize as the response body.</typeparam>
        /// <param name="req">The HTTP request data used to create the response.</param>
        /// <param name="result">The object to serialize and include in the response body.</param>
        /// <param name="statusCode">The HTTP status code to set for the response.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see
        /// cref="HttpResponseData"/> with the specified status code and JSON-serialized body.</returns>
        public static async Task<HttpResponseData> GetResultAsync<TParamType>(this HttpRequestData req, TParamType result, HttpStatusCode statusCode)
        {
            var response = req.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(result));
            return response;
        }

        /// <summary>
        /// Converts a Result object to HttpResponseData.
        /// </summary>
        public static async Task<HttpResponseData> ToHttpResponseDataAsync(this Result result, HttpRequestData req)
        {
            var response = req.CreateResponse(result.StatusCode);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(new { message = result.Message }));
            return response;
        }
    }
}
