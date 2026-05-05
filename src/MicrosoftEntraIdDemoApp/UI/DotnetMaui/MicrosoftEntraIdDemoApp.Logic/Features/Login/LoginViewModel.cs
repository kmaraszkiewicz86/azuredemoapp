using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicrosoftEntraIdDemoApp.Logic.Shared;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;
using System.Windows.Input;

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
                await navigationService.GoToUserCheckAsync();
            }
        }

        private async Task OnLoginAsync()
        {
            await loginHttpService.LoginAsync();
        }
    }
}
