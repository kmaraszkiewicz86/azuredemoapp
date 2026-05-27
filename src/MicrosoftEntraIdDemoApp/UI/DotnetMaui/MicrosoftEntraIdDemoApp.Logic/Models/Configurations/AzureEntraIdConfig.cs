namespace MicrosoftEntraIdDemoApp.Logic.Models.Configurations
{
    public class AzureEntraIdConfig
    {
        public static string Authority = "https://login.microsoftonline.com/{tenantId}/v2.0";
        public static string ClientId = "client_id";
        public static string TenantId = "tenant_id";
        public static string[] Scopes = new string[] { "openid", "offline_access" };
        public static string IOSKeyChainSecurityGroup = "com.microsoft.adalcache";
        public static object? ParentWindow { get; set; }
    }
}
