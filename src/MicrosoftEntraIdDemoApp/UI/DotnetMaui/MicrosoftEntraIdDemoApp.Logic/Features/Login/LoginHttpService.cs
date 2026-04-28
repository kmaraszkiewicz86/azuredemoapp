using Microsoft.Identity.Client;

namespace MicrosoftEntraIdDemoApp.Logic.Features.Login
{
    public class LoginHttpService(IPublicClientApplication identityClient): ILoginHttpService
    {
        public async Task<bool> LoginAsync()
        {
            try
            {
                // 1. Open the system browser for login (Microsoft login window appears)
                var authResult = await identityClient.AcquireTokenInteractive(["User.Read"])
                                           .ExecuteAsync();

                // 2. Token acquired! Save it in the device's secure storage
                await SecureStorage.Default.SetAsync("access_token", authResult.AccessToken);

                return true;
            }
            catch (MsalException ex)
            {
                // On the first run, 'ex.Message' will contain your actual Signature Hash 
                // if the one entered in Azure Portal is incorrect.
                Console.WriteLine($"MSAL Error: {ex.Message}");
                return false;
            }
        }
    }
}
