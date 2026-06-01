using FluentResults;
using MicrosoftEntraIdDemoApp.Logic.Extensions;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MicrosoftEntraIdDemoApp.Logic.Features.UserCheck
{
    public class AuthTestHttpService(HttpClient httpClient, ITokenService tokenService) : IAuthTestHttpService
    {
        public async Task<Result<UserCheckInfoDto>> GetDataAsync(AuthPageType authPageType)
        {
            var (isSuccess, token) = await tokenService.TryGetAsync();

            if (!isSuccess)
            {
                return Result.Fail("User is not logged in.");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/{authPageType}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage httpResponse = await httpClient.SendAsync(request);

            if (!httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    tokenService.Remove();
                    return Result.Fail("401");
                }

                // Use our extension to get the mapped message
                string errorMessage = httpResponse.ToUserFriendlyMessage();

                return Result.Fail(errorMessage);
            }

            UserCheckInfoDto? userCheckInfoDto = await httpResponse.Content.ReadFromJsonAsync<UserCheckInfoDto>();
            return Result.Ok(userCheckInfoDto!);
        }
    }
}
