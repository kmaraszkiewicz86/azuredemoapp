using FluentResults;
using Microsoft.Identity.Client;

namespace MicrosoftEntraIdDemoApp.Logic.Features.Login
{
    public class LoginHttpService(IPublicClientApplication identityClient): ILoginHttpService
    {
        public async Task<Result> LoginAsync()
        {
            try
            {
                // 1. Open the system browser for login (Microsoft login window appears)
                var authResult = await identityClient.AcquireTokenInteractive(["User.Read"])
                                           .ExecuteAsync();

                // 2. Token acquired! Save it in the device's secure storage
                await SecureStorage.Default.SetAsync("access_token", authResult.AccessToken);

                return Result.Ok();
            }
            catch (MsalException ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
