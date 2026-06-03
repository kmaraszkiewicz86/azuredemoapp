namespace MirsoftEntraDemo.ApiGateway.Models
{
    public class AzureAdOptions
    {
        public string Instance { get; set; } = default!;
        public string TenantId { get; set; } = default!;
        public string ClientId { get; set; } = default!;
        public string CallbackPath { get; set; } = default!;
        public string SignedOutCallbackPath { get; set; } = default!;
        public string ValidAudience { get; set; } = default!;
        public string ValidIssuer { get; set; } = default!;
    }
}
