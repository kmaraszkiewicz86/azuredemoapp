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
                    System.Net.HttpStatusCode.Unauthorized => "User not authenticated. Please login again.", // 401
                    System.Net.HttpStatusCode.Forbidden => "You don't have permission to access this resource.", // 403
                    System.Net.HttpStatusCode.NotFound => "The requested resource was not found.", // 404
                    System.Net.HttpStatusCode.InternalServerError => "Server error. Please try again later.", // 500
                    _ => $"Unexpected error: {response.ReasonPhrase} ({(int)response.StatusCode})"
                };
            }
        }
    }

}
