using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;
using System.Windows.Input;

namespace MicrosoftEntraIdDemoApp.Logic.ViewModels;

public sealed class AppShellViewModel : ObservableObject
{
    private readonly ITokenService _tokenService;

    public bool IsUserLogged
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            OnPropertyChanging(nameof(IsUserNotLogged));
        }
    }

    public bool IsUserNotLogged => !IsUserLogged;

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

        IsUserLogged = false;

        await Shell.Current.GoToAsync("//LoginPage");
    }
}