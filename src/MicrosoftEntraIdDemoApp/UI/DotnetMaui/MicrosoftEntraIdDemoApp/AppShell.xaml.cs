using MicrosoftEntraIdDemoApp.Logic.Features.AuthTest;
using MicrosoftEntraIdDemoApp.Logic.Features.UserCheck;
using MicrosoftEntraIdDemoApp.Logic.ViewModels;

namespace MicrosoftEntraIdDemoApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = IPlatformApplication.Current!
                .Services
                .GetRequiredService<AppShellViewModel>();

            Routing.RegisterRoute(nameof(UserCheckPage), typeof(UserCheckPage));
        }
    }
}
