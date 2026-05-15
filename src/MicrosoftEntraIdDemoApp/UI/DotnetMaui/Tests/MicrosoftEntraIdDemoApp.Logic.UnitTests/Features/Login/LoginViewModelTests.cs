using FluentAssertions;
using FluentResults;
using MicrosoftEntraIdDemoApp.Logic.UnitTests.Fixtures;
using NSubstitute;

namespace MicrosoftEntraIdDemoApp.Logic.Features.UnitTests.Login
{
    public class LoginViewModelTests
    {
        private LoginViewModelFixture _fixture;

        public LoginViewModelTests()
        {
            _fixture = new LoginViewModelFixture();
        }

        [Fact]
        public async Task OnLoadAsync_WhenUserIsLogged_ShouldNavigateToUserCheck()
        {
            // Arrange
            _fixture.TokenService!.IsUserLogged().Returns(Task.FromResult(true));
            var viewModel = _fixture.CreateFixure();

            // Act
            await viewModel.LoadCommand.ExecuteAsync(null);

            // Assert
            await _fixture.NavigationService!.Received(1).GoToUserCheckAsync();
            await _fixture.TokenService!.Received(1).IsUserLogged();
            viewModel.ErrorMessage.Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task OnLoadAsync_WhenUserIsNotLogged_ShouldNotNavigate()
        {
            // Arrange
            _fixture.TokenService!.IsUserLogged().Returns(Task.FromResult(false));
            var viewModel = _fixture.CreateFixure();

            // Act
            await viewModel.LoadCommand.ExecuteAsync(null);

            // Assert
            await _fixture.NavigationService!.DidNotReceive().GoToUserCheckAsync();
            await _fixture.TokenService!.Received(1).IsUserLogged();
            viewModel.ErrorMessage.Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task OnLoginAsync_WhenUserIsLogged_ShouldNavigateToUserCheck()
        {
            // Arrange
            _fixture.LoginHttpService!.LoginAsync().Returns(Task.FromResult(Result.Ok()));
            var viewModel = _fixture.CreateFixure();

            // Act
            await viewModel.LoginCommand.ExecuteAsync(null);

            // Assert
            await _fixture.NavigationService!.Received(1).GoToUserCheckAsync();
            await _fixture.LoginHttpService!.Received(1).LoginAsync();
            viewModel.ErrorMessage.Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task OnLoginAsync_WhenUserIsNotLogged_ShouldNotNavigate()
        {
            // Arrange
            _fixture.LoginHttpService!.LoginAsync().Returns(Task.FromResult(Result.Fail("Login failed")));
            var viewModel = _fixture.CreateFixure();

            // Act
            await viewModel.LoginCommand.ExecuteAsync(null);

            // Assert
            await _fixture.NavigationService!.DidNotReceive().GoToUserCheckAsync();
            await _fixture.LoginHttpService!.Received(1).LoginAsync();
            viewModel.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        }
    }
}
