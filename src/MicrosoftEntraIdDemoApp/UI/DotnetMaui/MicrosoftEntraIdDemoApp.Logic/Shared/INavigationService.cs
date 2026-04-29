namespace MicrosoftEntraIdDemoApp.Logic.Shared
{
    public interface INavigationService
    {
        Task GoToUserCheckAsync();
        Task GoToLoginAsync();
        Task GoBackAsync();
    }
}
