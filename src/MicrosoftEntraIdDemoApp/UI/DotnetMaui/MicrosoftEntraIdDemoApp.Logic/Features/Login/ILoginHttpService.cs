using FluentResults;

namespace MicrosoftEntraIdDemoApp.Logic.Features.Login
{
    public interface ILoginHttpService
    {
        Task<Result> LoginAsync();
    }
}
