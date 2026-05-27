using FluentResults;
using Microsoft.Identity.Client;
using MicrosoftEntraIdDemoApp.Logic.Models.Configurations;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;

namespace MicrosoftEntraIdDemoApp.Logic.Features.Login
{
    public class LoginHttpService(IPublicClientApplication publicClientApplication, ITokenService tokenService) : ILoginHttpService
    {
        public async Task<Result> LoginAsync()
        {
            try
            {
                // 1. Open the system browser for login (Microsoft login window appears)
                var authResult = await publicClientApplication.AcquireTokenInteractive(AzureEntraIdConfig.Scopes)
                    .WithParentActivityOrWindow(AzureEntraIdConfig.ParentWindow)
                    .ExecuteAsync();

                // 2. Token acquired! Save it in the device's secure storage
                await tokenService.SaveAsync(authResult.AccessToken);

                return Result.Ok();
            }
            catch (MsalException ex)
            {
                return Result.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Unhandled error occurred: {ex.Message}");
            }
        }
    }
}
