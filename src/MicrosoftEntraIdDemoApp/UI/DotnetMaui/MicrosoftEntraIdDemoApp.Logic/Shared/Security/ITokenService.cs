namespace MicrosoftEntraIdDemoApp.Logic.Shared.Security
{
    public interface ITokenService
    {
        Task<string> GetAsync();
        Task<bool> IsUserLogged();
        Task SaveAsync(string token);
    }
}
