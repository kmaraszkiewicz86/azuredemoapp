using MicrosoftEntraIdDemoApp.Logic.Features.Login;

namespace MicrosoftEntraIdDemoApp.Logic.Features;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    //it is created temporialy before I update the Kmaraszkiewicz86.Maui.Behaviors to support .net 10.0
    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (BindingContext is LoginViewModel viewModel)
        {
            viewModel.LoadCommand.Execute(null);
        }
    }
}