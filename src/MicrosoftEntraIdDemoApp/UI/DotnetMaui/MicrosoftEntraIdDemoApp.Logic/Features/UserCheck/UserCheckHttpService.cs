using CommunityToolkit.Mvvm.ComponentModel;
using FluentResults;
using MicrosoftEntraIdDemoApp.Logic.Extensions;
using System.Net.Http.Json;

namespace MicrosoftEntraIdDemoApp.Logic.Features.UserCheck
{
    public class UserCheckHttpService(HttpClient httpClient) : IUserCheckHttpService
    {
        public async Task<Result<UserCheckInfoDto>> GetDataAsync()
        {

            using HttpResponseMessage httpResponse = await httpClient.GetAsync("/bff/user");

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
