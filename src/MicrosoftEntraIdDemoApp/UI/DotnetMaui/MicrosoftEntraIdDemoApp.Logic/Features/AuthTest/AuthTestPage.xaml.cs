using MicrosoftEntraIdDemoApp.Logic.Features.UserCheck;

namespace MicrosoftEntraIdDemoApp.Logic.Features.AuthTest;

public partial class AuthTestPage : ContentPage
{
	public AuthTestPage(AuthTestViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}