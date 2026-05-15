using AutoFixture;
using MicrosoftEntraIdDemoApp.Logic.Features.Login;
using MicrosoftEntraIdDemoApp.Logic.Shared;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;

namespace MicrosoftEntraIdDemoApp.Logic.UnitTests.Fixtures
{
    public class LoginViewModelFixture : TestBase<LoginViewModel>
    {
        public ILoginHttpService? LoginHttpService { get; init; }
        public ITokenService? TokenService { get; init; }
        public INavigationService? NavigationService { get; init; }

        public LoginViewModelFixture()
        {
            LoginHttpService = Fixture.Freeze<ILoginHttpService>();
            TokenService = Fixture.Freeze<ITokenService>();
            NavigationService = Fixture.Freeze<INavigationService>();
        }

        public override LoginViewModel CreateFixure()
            => Fixture.Build<LoginViewModel>()
                .OmitAutoProperties()
                .Create();
    }
}
