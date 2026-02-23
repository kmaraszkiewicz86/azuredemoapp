using System.Net;

namespace AzureJsonDataFlowFunction.Models.Dto
{
    public class ResultWithValue<T> : Result
    {
        public T Value { get; }

        public ResultWithValue(bool isSuccess, string message, HttpStatusCode statusCode, T value)
            : base(isSuccess, message, statusCode)
        {
            Value = value;
        }

        public static ResultWithValue<T> Ok(T value) =>
            new (true, string.Empty, HttpStatusCode.OK, value);

        public new static ResultWithValue<T> BadRequest(string message) =>
            new (false, message, HttpStatusCode.BadRequest, default!);

        public new static ResultWithValue<T> InternalServerError(string message) =>
            new (false, message, HttpStatusCode.InternalServerError, default!);
    }
}
        