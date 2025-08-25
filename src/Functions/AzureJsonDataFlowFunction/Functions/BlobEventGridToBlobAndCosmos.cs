using Azure.Messaging.EventGrid;
using AzureJsonDataFlowFunction.Models.Dto;
using AzureJsonDataFlowFunction.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureJsonDataFlowFunction.Functions
{
    /// <summary>
    /// Processes Event Grid events triggered by blob creation, downloads the blob content,  and stores the event
    /// metadata and blob content in Azure Cosmos DB.
    /// </summary>
    public class BlobEventGridToBlobAndCosmos
    {
        private readonly IEventGridToCosmosService _eventGridToCosmosService;
        private readonly ILogger<BlobEventGridToBlobAndCosmos> _logger;

        public BlobEventGridToBlobAndCosmos(IEventGridToCosmosService eventGridToCosmosService, ILogger<BlobEventGridToBlobAndCosmos> logger)
        {
            _eventGridToCosmosService = eventGridToCosmosService;
            _logger = logger;
        }

        /// <summary>
        /// Handles an Event Grid event, processes it, and get the resulting data from a blob storage and save it to a Cosmos
        /// DB instance.
        /// </summary>
        /// <remarks>This method triggers on an Event Grid event, processes the event data, and logs the
        /// outcome.  If the processing is successful, the data is stored in the appropriate destinations, and an
        /// informational log is written.  If the processing fails, an error log is written with details about the
        /// failure.</remarks>
        /// <param name="eventGridEvent">The Event Grid event to process. This parameter cannot be null.</param>
        /// <returns></returns>
        [Function("BlobEventGridToBlobAndCosmos")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            try
            {
                _logger.LogInformation("Starting processing an event grid...");
                Result result = await _eventGridToCosmosService.ProcessEventAsync(eventGridEvent);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Event processed and data stored successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to process event with status code: {result.StatusCode} and message {result.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error logging function trigger: {ex.Message}");
            }
        }
    }
}