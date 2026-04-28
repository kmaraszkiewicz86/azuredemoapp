using FluentResults;

namespace MicrosoftEntraIdDemoApp.Logic.Features.UserCheck
{
    public interface IUserCheckHttpService
    {
        Task<Result<UserCheckInfoDto>> GetDataAsync();
    }
}
