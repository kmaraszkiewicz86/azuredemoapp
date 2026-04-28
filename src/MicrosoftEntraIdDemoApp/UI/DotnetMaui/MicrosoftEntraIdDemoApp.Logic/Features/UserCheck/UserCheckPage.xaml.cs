namespace MicrosoftEntraIdDemoApp.Logic.Features.UserCheck;

public partial class UserCheckPage : ContentPage
{
	public UserCheckPage(UserCheckViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}