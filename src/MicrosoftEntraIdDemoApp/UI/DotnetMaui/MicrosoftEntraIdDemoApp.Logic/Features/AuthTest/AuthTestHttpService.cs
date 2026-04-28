using FluentResults;
using MicrosoftEntraIdDemoApp.Logic.Extensions;
using System.Net.Http.Json;

namespace MicrosoftEntraIdDemoApp.Logic.Features.UserCheck
{
    public class AuthTestHttpService(HttpClient httpClient) : IAuthTestHttpService
    {
        public async Task<Result<UserCheckInfoDto>> GetDataAsync(AuthPageType authPageType)
        {
            using HttpResponseMessage httpResponse = await httpClient.GetAsync($"/api/{authPageType}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                // Use our extension to get the mapped message
                string errorMessage = httpResponse.ToUserFriendlyMessage();

                return Result.Fail(errorMessage);
            }

            UserCheckInfoDto? userCheckInfoDto = await httpResponse.Content.ReadFromJsonAsync<UserCheckInfoDto>();
            return Result.Ok(userCheckInfoDto!);
        }
    }
}
