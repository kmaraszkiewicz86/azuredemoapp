namespace MicrosoftEntraIdDemoApp.Logic.Models.Configurations
{
    public record AzureEntraIdConfig(string ClientId, string TenantId, string RedirectUri);
}
