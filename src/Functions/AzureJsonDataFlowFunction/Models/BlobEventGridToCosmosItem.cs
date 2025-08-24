namespace AzureJsonDataFlowFunction.Models
{
    public class BlobEventGridToCosmosItem
    {
        public string Id { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public DateTimeOffset EventTime { get; set; }
        public string BlobUrl { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public JsonModel JsonModel { get; set; } = default!;
    }
}
