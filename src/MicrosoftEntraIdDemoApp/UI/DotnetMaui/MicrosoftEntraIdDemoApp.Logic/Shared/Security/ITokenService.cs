namespace MicrosoftEntraIdDemoApp.Logic.Shared.Security
{
    public interface ITokenService
    {
        Task<string> GetAsync();
        Task<bool> IsUserLogged();
        void Remove();
        Task SaveAsync(string token);
        Task<(bool IsSuccess, string Token)> TryGetAsync();
    }
}
