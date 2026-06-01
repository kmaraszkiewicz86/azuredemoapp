using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;
using System.Windows.Input;

namespace MicrosoftEntraIdDemoApp.Logic.ViewModels;

public sealed class AppShellViewModel : ObservableObject
{
    private readonly ITokenService _tokenService;

    public AppShellViewModel(
        ITokenService tokenService)
    {
        _tokenService = tokenService;

        LogoutCommand = new AsyncRelayCommand(LogoutAsync);
    }

    public ICommand LogoutCommand { get; }

    private async Task LogoutAsync()
    {
        _tokenService.Remove();

        await Shell.Current.GoToAsync("//LoginPage");
    }
}