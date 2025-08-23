using Azure.Messaging.EventGrid;
using System.Threading.Tasks;

namespace AzureJsonDataFlowFunction.Services
{
    /// <summary>
    /// Service interface for processing Event Grid events and saving blob content to Cosmos DB.
    /// </summary>
    public interface IEventGridToCosmosService : IService
    {
        Task ProcessEventAsync(EventGridEvent eventGridEvent);
    }
}