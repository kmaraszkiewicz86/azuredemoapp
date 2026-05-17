using FluentResults;
using Microsoft.Identity.Client;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;
using System.Diagnostics;

namespace MicrosoftEntraIdDemoApp.Logic.Features.Login
{
    public class LoginHttpService(IPublicClientApplication identityClient, ITokenService tokenService) : ILoginHttpService
    {
        public async Task<Result> LoginAsync()
        {
            try
            {
#if ANDROID
                // Android: upewnij się że parent activity jest dostępna
                if (Platform.CurrentActivity == null)
                {
                    return Result.Fail("Platform.CurrentActivity is null");
                }
#endif

                var authResult = await identityClient
                    .AcquireTokenInteractive(new[] { "User.Read" })
                    .ExecuteAsync();

                await tokenService.SaveAsync(authResult.AccessToken);

                return Result.Ok();
            }
            catch (MsalException ex)
            {
                Debug.WriteLine($"MSAL ERROR: {ex.ErrorCode} - {ex.Message}");
                return Result.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}\n{ex.StackTrace}");
                return Result.Fail(ex.Message);
            }
        }
    }
}
