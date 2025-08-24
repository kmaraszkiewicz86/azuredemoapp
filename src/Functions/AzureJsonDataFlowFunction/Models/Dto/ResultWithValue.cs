using System.Net;

namespace AzureJsonDataFlowFunction.Models.Dto
{
    public class ResultWithValue<T> : Result
    {
        public T Value { get; }

        public ResultWithValue(string message, HttpStatusCode statusCode, T value)
            : base(message, statusCode)
        {
            Value = value;
        }

        public static ResultWithValue<T> Ok(T value) =>
            new (string.Empty, HttpStatusCode.OK, value);

        public new static ResultWithValue<T> BadRequest(string message) =>
            new (message, HttpStatusCode.BadRequest, default!);

        public new static ResultWithValue<T> InternalServerError(string message) =>
            new (message, HttpStatusCode.InternalServerError, default!);
    }
}
        