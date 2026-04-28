using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace MicrosoftEntraIdDemoApp.Logic.Features.UserCheck
{
    public class AuthTestViewModel(IAuthTestHttpService authTestHttpService) : ObservableObject
    {
        public string ErrorMessage
        {
            get => field;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public UserCheckInfoDto? UserCheckInfo
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        // Property to bind to a Picker or pass from UI
        public AuthPageType SelectedPageType
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public ICommand GetDataCommand => new AsyncRelayCommand(GetDataAsync);

        public async Task GetDataAsync()
        {
            // Clear previous errors and data
            ErrorMessage = string.Empty;
            UserCheckInfo = null;

            // Call the service passing the selected enum value
            var result = await authTestHttpService.GetDataAsync(SelectedPageType);

            if (result.IsFailed)
            {
                // Map FluentResults (or your custom Result) errors to the UI
                ErrorMessage = string.Join(Environment.NewLine, result.Errors.Select(e => e.Message));
                return;
            }

            // Update the UI on success
            UserCheckInfo = result.Value;
        }
    }
}
