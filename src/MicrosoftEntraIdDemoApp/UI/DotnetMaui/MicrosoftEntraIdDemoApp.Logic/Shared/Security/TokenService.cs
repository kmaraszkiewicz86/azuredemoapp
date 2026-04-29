namespace MicrosoftEntraIdDemoApp.Logic.Shared.Security
{
    public class TokenService : ITokenService
    {
        public const string TokenKey = "api_token";

        /// <summary>
        /// Saves the token to the secure storage
        /// </summary>
        /// <param name="token">the api token to storage</param>
        /// <returns></returns>
        public async Task SaveAsync(string token)
        {
            await SecureStorage.Default.SetAsync(TokenKey, token);
        }

        /// <summary>
        /// Get token if exist, otherwise return empty string
        /// </summary>
        /// <returns>The api token</returns>
        public async Task<string> GetAsync()
        {
            return await SecureStorage.Default.GetAsync(TokenKey) ?? string.Empty;
        }

        /// <summary>
        /// Checks if token is saved in storage to check if user is logged in to the system
        /// </summary>
        /// <returns>True if user is logged otherwise false</returns>
        public async Task<bool> IsUserLogged()
        {
            string token = await GetAsync();

            return !string.IsNullOrWhiteSpace(token);
        }
    }
}
