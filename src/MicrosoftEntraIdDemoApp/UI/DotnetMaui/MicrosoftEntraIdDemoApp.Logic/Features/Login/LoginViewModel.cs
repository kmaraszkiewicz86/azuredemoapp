using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace MicrosoftEntraIdDemoApp.Logic.Features.Login
{
    public class LoginViewModel(ILoginHttpService loginHttpService) : ObservableObject
    {
        public string ErrorMessage
        {
            get => field;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public ICommand LoginCommand => new AsyncRelayCommand(OnLoginAsync);

        private async Task OnLoginAsync()
        {
            await loginHttpService.LoginAsync();
        }
    }
}
