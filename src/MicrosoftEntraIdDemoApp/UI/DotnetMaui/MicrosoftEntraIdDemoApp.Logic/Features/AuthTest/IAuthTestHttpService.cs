using FluentResults;

namespace MicrosoftEntraIdDemoApp.Logic.Features.UserCheck
{
    public interface IAuthTestHttpService
    {
        Task<Result<UserCheckInfoDto>> GetDataAsync(AuthPageType authPageType);
    }
}
