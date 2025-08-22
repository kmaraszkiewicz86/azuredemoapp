using System.Net;

namespace AzureJsonDataFlowFunction.Models.Dto
{
    public class Result
    {
        public string Message { get; } = string.Empty;
        public HttpStatusCode StatusCode { get; }

        protected Result(string message, HttpStatusCode statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }

        public static Result Ok(string message) =>
            new (message, HttpStatusCode.OK);

        public static Result BadRequest(string message) =>
            new (message, HttpStatusCode.BadRequest);

        public static Result InternalServerError(string message) =>
            new (message, HttpStatusCode.InternalServerError);
    }
}