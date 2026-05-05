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
            _fixture.TokenService.IsUserLogged().Returns(Task.FromResult(true));
            var viewModel = _fixture.CreateFixure();

            // Act
            await viewModel.LoadCommand.ExecuteAsync(null);

            // Assert
            await _fixture.NavigationService.Received(1).GoToUserCheckAsync();
            await _fixture.TokenService.Received(1).IsUserLogged();
        }

        [Fact]
        public async Task OnLoadAsync_WhenUserIsNotLogged_ShouldNotNavigate()
        {
            // Arrange
            _fixture.TokenService.IsUserLogged().Returns(Task.FromResult(false));
            var viewModel = _fixture.CreateFixure();

            // Act
            await viewModel.LoadCommand.ExecuteAsync(null);

            // Assert
            await _fixture.NavigationService.DidNotReceive().GoToUserCheckAsync();
            await _fixture.TokenService.Received(1).IsUserLogged();
        }

        [Fact]
        public async Task OnLoadAsync_WhenTokenServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var exception = new InvalidOperationException("Token service error");
            _fixture.TokenService.IsUserLogged().Returns(Task.FromException<bool>(exception));
            var viewModel = _fixture.CreateFixure();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => viewModel.LoadCommand.ExecuteAsync(null)
            );
        }
    }
}
