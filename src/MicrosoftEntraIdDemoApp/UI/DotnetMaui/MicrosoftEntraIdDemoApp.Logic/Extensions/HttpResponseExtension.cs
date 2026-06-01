using System.Net;

namespace MicrosoftEntraIdDemoApp.Logic.Extensions
{
    public static class HttpResponseExtension
    {
        extension(HttpResponseMessage response)
        {
            /// <summary>
            /// Extension method to map HTTP status codes to human-readable messages
            /// </summary>
            /// <returns></returns>
            public string ToUserFriendlyMessage()
            {
                return response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => "Bad request. Please check your input.",
                    HttpStatusCode.Unauthorized => "Unauthorized. Please log in again.",
                    HttpStatusCode.Forbidden => "Forbidden. You don't have permission to access this resource.",
                    HttpStatusCode.NotFound => "Not found. The requested resource does not exist.",
                    HttpStatusCode.InternalServerError => "Internal server error. Please try again later.",
                    _ => $"An error occurred: {response.StatusCode}"
                };
            }
        }
    }

}
