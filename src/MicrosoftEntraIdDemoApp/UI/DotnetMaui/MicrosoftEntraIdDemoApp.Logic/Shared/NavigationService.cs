using MicrosoftEntraIdDemoApp.Logic.Features;
using MicrosoftEntraIdDemoApp.Logic.Features.UserCheck;

namespace MicrosoftEntraIdDemoApp.Logic.Shared
{
    public class NavigationService : INavigationService
    {
        // Resets stack and goes to UserCheck
        public async Task GoToUserCheckAsync()
            => await Shell.Current.GoToAsync($"//{nameof(UserCheckPage)}");

        // Resets stack and goes to Login
        public async Task GoToLoginAsync()
            => await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}
