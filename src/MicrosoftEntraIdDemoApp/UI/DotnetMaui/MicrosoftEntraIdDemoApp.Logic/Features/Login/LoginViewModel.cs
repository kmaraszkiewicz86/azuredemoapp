using FluentResults;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicrosoftEntraIdDemoApp.Logic.Shared;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;

namespace MicrosoftEntraIdDemoApp.Logic.Features.Login
{
    public class LoginViewModel(ILoginHttpService loginHttpService, ITokenService tokenService, INavigationService navigationService) : ObservableObject
    {
        public string ErrorMessage
        {
            get => field;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public IAsyncRelayCommand LoginCommand => new AsyncRelayCommand(OnLoginAsync);

        public IAsyncRelayCommand LoadCommand => new AsyncRelayCommand(OnLoadAsync);

        private async Task OnLoadAsync()
        {
            if (await tokenService.IsUserLogged())
            {
                await RedirectToUserCheckPageAsync();
            }
        }

        private async Task OnLoginAsync()
        {
            Result result = await loginHttpService.LoginAsync();
            if (result.IsSuccess)
            {
                await RedirectToUserCheckPageAsync();
            }
            else
            {
                var errorMessages = result.Errors?.Select(e => e.Message) ?? [];
                ErrorMessage = errorMessages.Any() ? string.Join(';', errorMessages) : "Login failed - unknown error";
            }
        }

        private async Task RedirectToUserCheckPageAsync()
        {
            try
            {
                await navigationService.GoToUserCheckAsync();
            }
            catch
            {
                ErrorMessage = "An error occurred while navigate to the user check page";
            }
        }

    }
}
