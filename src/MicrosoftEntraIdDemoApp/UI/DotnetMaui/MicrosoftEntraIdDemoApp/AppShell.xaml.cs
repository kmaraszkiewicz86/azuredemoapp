using MicrosoftEntraIdDemoApp.Logic.Features.UserCheck;

namespace MicrosoftEntraIdDemoApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(UserCheckPage), typeof(UserCheckPage));
        }
    }
}
