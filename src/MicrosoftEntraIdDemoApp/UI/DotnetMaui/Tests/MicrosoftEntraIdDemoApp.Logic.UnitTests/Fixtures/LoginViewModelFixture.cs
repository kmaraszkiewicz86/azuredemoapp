using AutoFixture;
using MicrosoftEntraIdDemoApp.Logic.Features.Login;
using MicrosoftEntraIdDemoApp.Logic.Shared;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;

namespace MicrosoftEntraIdDemoApp.Logic.UnitTests.Fixtures
{
    public class LoginViewModelFixture : TestBase<LoginViewModel>
    {
        public ILoginHttpService LoginHttpService => Fixture.Create<ILoginHttpService>();
        public ITokenService TokenService => Fixture.Create<ITokenService>();
        public INavigationService NavigationService => Fixture.Create<INavigationService>();
    }
}
