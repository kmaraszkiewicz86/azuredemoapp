using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentResults;
using Microsoft.Identity.Client;
using MicrosoftEntraIdDemoApp.Logic.Models.Configurations;
using MicrosoftEntraIdDemoApp.Logic.Shared;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;
using MicrosoftEntraIdDemoApp.Logic.ViewModels;

namespace MicrosoftEntraIdDemoApp.Logic.Features.Login
{
    public class LoginViewModel(ILoginHttpService loginHttpService, ITokenService tokenService, INavigationService navigationService, AppShellViewModel appShellViewModel) : ObservableObject
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
            appShellViewModel.IsUserLogged = false;

            if (await tokenService.IsUserLogged())
            {
                appShellViewModel.IsUserLogged = true;

                await RedirectToUserCheckPageAsync();
            }
        }

        private async Task OnLoginAsync()
        {
            appShellViewModel.IsUserLogged = false;

            Result result = await loginHttpService.LoginAsync();
            if (result.IsSuccess)
            {
                appShellViewModel.IsUserLogged = true;
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
