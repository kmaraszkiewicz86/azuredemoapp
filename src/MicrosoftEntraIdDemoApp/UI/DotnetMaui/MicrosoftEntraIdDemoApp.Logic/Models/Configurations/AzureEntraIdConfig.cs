namespace MicrosoftEntraIdDemoApp.Logic.Models.Configurations
{
    public class AzureEntraIdConfig
    {
        /// <summary>
        /// Microsoft Entra ID authority endpoint.
        /// Replace {TenantId} with your Directory (tenant) ID.
        ///
        /// Azure Portal:
        /// Entra ID -> Overview -> Tenant ID
        /// </summary>
        public static string Authority =
            "https://login.microsoftonline.com/{TenantId}/v2.0";

        /// <summary>
        /// Application (client) ID of the App Registration.
        ///
        /// Azure Portal:
        /// Entra ID -> App registrations -> Your Application -> Overview
        /// -> Application (client) ID
        /// </summary>
        public static string ClientId =
            "{application_client_id}";

        /// <summary>
        /// Directory (tenant) ID.
        ///
        /// Azure Portal:
        /// Entra ID -> Overview
        /// -> Tenant ID
        /// </summary>
        public static string TenantId =
            "{tenant_id}";

        /// <summary>
        /// API scopes requested by the mobile application.
        ///
        /// Azure Portal:
        /// Entra ID -> App registrations -> Your Application
        /// -> Expose an API
        /// -> Scope name (e.g. access_as_user)
        ///
        /// Example:
        /// api://fb631f30-6681-478c-9b4b-48100cce3fea/access_as_user
        ///
        /// NOTE:
        /// Do not use only "openid" or "offline_access" if you want
        /// to call your own API. Request the API scope instead.
        /// </summary>
        public static string[] Scopes =
        [
            "api://{application_client_id}/access_as_user"
        ];

        /// <summary>
        /// iOS Keychain security group used by MSAL token cache.
        /// Usually the default value is sufficient unless you need
        /// to share authentication state between multiple iOS apps.
        /// </summary>
        public static string IOSKeyChainSecurityGroup =
            "com.microsoft.adalcache";

        /// <summary>
        /// Parent window required by MSAL on some platforms
        /// when displaying the interactive login dialog.
        /// </summary>
        public static object? ParentWindow { get; set; }
    }
}
