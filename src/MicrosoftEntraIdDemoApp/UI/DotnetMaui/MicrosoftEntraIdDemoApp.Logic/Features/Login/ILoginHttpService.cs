namespace MicrosoftEntraIdDemoApp.Logic.Features.Login
{
    public interface ILoginHttpService
    {
        Task<bool> LoginAsync();
    }
}
