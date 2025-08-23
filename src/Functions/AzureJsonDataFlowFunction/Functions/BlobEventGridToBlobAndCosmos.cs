using Azure.Messaging.EventGrid;
using AzureJsonDataFlowFunction.Services;
using Microsoft.Azure.Functions.Worker;

namespace AzureJsonDataFlowFunction.Functions
{
    /// <summary>
    /// Processes Event Grid events triggered by blob creation, downloads the blob content,  and stores the event
    /// metadata and blob content in Azure Cosmos DB.
    /// </summary>
    public class BlobEventGridToBlobAndCosmos
    {
        private readonly IEventGridToCosmosService _eventGridToCosmosService;

        public BlobEventGridToBlobAndCosmos(IEventGridToCosmosService eventGridToCosmosService)
        {
            _eventGridToCosmosService = eventGridToCosmosService;
        }

        [Function("BlobEventGridToBlobAndCosmos")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            await _eventGridToCosmosService.ProcessEventAsync(eventGridEvent);
        }
    }
}