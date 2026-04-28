using MicrosoftEntraIdDemoApp.Logic.Features.Login;

namespace MicrosoftEntraIdDemoApp.Logic.Features;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}