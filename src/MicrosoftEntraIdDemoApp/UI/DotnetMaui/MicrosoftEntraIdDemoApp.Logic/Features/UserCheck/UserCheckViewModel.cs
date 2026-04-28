using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentResults;
using System.Windows.Input;

namespace MicrosoftEntraIdDemoApp.Logic.Features.UserCheck
{
    public class UserCheckViewModel(IUserCheckHttpService userCheckHttpService) : ObservableObject
    {
        public string ErrorMessage
        {
            get => field;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public UserCheckInfoDto? UserCheckInfo
        {
            get => field; 
            set => SetProperty(ref field, value); 
        }

        public ICommand GetDataCommand => new AsyncRelayCommand(GetDataAsync);

        public async Task GetDataAsync()
        {
            Result<UserCheckInfoDto> userCheckInfoResult = await userCheckHttpService.GetDataAsync();

            if (userCheckInfoResult.IsFailed)
            {
                ErrorMessage = string.Join(',', userCheckInfoResult.Errors.Select(e => e.Message));
                return;
            }

            UserCheckInfo = userCheckInfoResult.Value;
        }
    }
}
