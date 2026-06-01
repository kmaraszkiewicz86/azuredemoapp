using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentResults;
using MicrosoftEntraIdDemoApp.Logic.Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MicrosoftEntraIdDemoApp.Logic.Features.UserCheck
{
    public class RoleOption
    {
        public int Value { get; set; }

        public string Name { get; set; }
    }

    public class AuthTestViewModel(IAuthTestHttpService authTestHttpService, INavigationService navigationService) : ObservableObject
    {
        public string ErrorMessage
        {
            get => field;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public string ResponseMessage
        {
            get => field;
            set
            {
                SetProperty(ref field, value);
                OnPropertyChanged(nameof(ResponseMessage));
            }
        } = string.Empty;

        public bool IsResponseVisible => !string.IsNullOrEmpty(ResponseMessage);

        // Property to bind to a Picker or pass from UI
        public RoleOption? SelectedPageType
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public ObservableCollection<RoleOption> Roles
        {
            get
            {
                return new ObservableCollection<RoleOption>([
                    new RoleOption { Value = 1, Name = "Admin" },
                    new RoleOption { Value = 2, Name = "User" },
                    new RoleOption { Value = 3, Name = "Tester" }]
                );
            }
        }

        public ICommand GetDataCommand => new AsyncRelayCommand(GetDataAsync);

        public async Task GetDataAsync()
        {
            // Clear previous errors and data
            ErrorMessage = string.Empty;
            ResponseMessage = string.Empty;

            if (SelectedPageType is null || SelectedPageType.Value == 0)
            {
                ErrorMessage = "Please select a role to check.";
                return;
            }

            // Call the service passing the selected enum value
            Result<string> result = await authTestHttpService.GetDataAsync((AuthPageType)SelectedPageType.Value);

            if (result.IsFailed)
            {
                // Map FluentResults (or your custom Result) errors to the UI
                ErrorMessage = string.Join(Environment.NewLine, result.Errors.Select(e => e.Message));
                return;
            }

            // Update the UI on success
            ResponseMessage = result.Value;
        }
    }
}
