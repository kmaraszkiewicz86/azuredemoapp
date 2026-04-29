namespace MicrosoftEntraIdDemoApp.Logic.Shared
{
    public class NavigationService : INavigationService
    {
        // Resets stack and goes to UserCheck
        public async Task GoToUserCheckAsync()
            => await Shell.Current.GoToAsync("//UserCheck");

        // Resets stack and goes to Login
        public async Task GoToLoginAsync()
            => await Shell.Current.GoToAsync("//Login");

        // Standard back navigation
        public async Task GoBackAsync()
            => await Shell.Current.GoToAsync("..");
    }
}
