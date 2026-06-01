using FluentResults;

namespace MicrosoftEntraIdDemoApp.Logic.Features.UserCheck
{
    public interface IAuthTestHttpService
    {
        Task<Result<string>> GetDataAsync(AuthPageType authPageType);
    }
}
