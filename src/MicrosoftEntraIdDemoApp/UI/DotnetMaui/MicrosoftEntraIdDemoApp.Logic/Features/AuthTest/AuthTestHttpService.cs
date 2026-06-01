using FluentResults;
using MicrosoftEntraIdDemoApp.Logic.Extensions;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MicrosoftEntraIdDemoApp.Logic.Features.UserCheck
{
    public class AuthTestHttpService(HttpClient httpClient, ITokenService tokenService) : IAuthTestHttpService
    {
        public async Task<Result<string>> GetDataAsync(AuthPageType authPageType)
        {
            var (isSuccess, token) = await tokenService.TryGetAsync();

            if (!isSuccess)
            {
                return Result.Fail("User is not logged in.");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/{authPageType.ToString().ToLower()}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage httpResponse = await httpClient.SendAsync(request);

            if (!httpResponse.IsSuccessStatusCode)
            {
                // Use our extension to get the mapped message
                string errorMessage = httpResponse.ToUserFriendlyMessage();

                return Result.Fail(errorMessage);
            }

            AuthTestDto? data = await httpResponse.Content.ReadFromJsonAsync<AuthTestDto>();
            return Result.Ok(data?.Message ?? "No message available.");
        }
    }
}
