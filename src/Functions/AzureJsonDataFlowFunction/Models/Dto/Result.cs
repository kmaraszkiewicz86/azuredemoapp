using System.Net;

namespace AzureJsonDataFlowFunction.Models.Dto
{
    public class Result
    {
        public string Message { get; } = string.Empty;
        public HttpStatusCode StatusCode { get; }
        public bool IsSuccess { get; }

        protected Result(bool isSuccess, string message, HttpStatusCode statusCode)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = statusCode;
        }

        public static Result Ok(string message) =>
            new (true, message, HttpStatusCode.OK);

        public static Result BadRequest(string message) =>
            new (false, message, HttpStatusCode.BadRequest);

        public static Result InternalServerError(string message) =>
            new (false, message, HttpStatusCode.InternalServerError);
    }
}