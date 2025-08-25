using Azure.Storage.Blobs;
using AzureJsonDataFlowFunction.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AzureJsonDataFlowFunction.Extensions
{
    public static class DependencyInjectionExtnsion
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, Assembly? assembly = null)
        {
            RegisterAllByMarkerInterface<IService>(services, assembly);

            return services;
        }

        /// <summary>
        /// Registers BlobServiceClient and CosmosClient in DI.
        /// </summary>
        public static IServiceCollection RegisterExternalAzureClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(x =>
            {
                var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                return new BlobServiceClient(connectionString);
            });

            services.AddSingleton<CosmosClient>(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                string endpointUri = "https://demokmar.documents.azure.com:443/";// config["CosmosDb:Endpoint"]!;
                string primaryKey = "8t5AaKP8ky799xvUAfeXZ9W8wEs8uIYqr2LFMcD914DJustbmoEMbqUeKbGiMFWhtY7arypn7GVNACDbdn6m9w==";// config["CosmosDb:PrimaryKey"]!;
                return new CosmosClient(endpointUri, primaryKey);
            });

            return services;
        }

        /// <summary>
        /// Generic method that registers all interfaces inheriting from the marker interface and their implementations.
        /// </summary>
        private static void RegisterAllByMarkerInterface<TMarker>(
            IServiceCollection services,
            Assembly? assembly = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TMarker : class
        {
            assembly ??= Assembly.GetExecutingAssembly();

            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsInterface && typeof(TMarker).IsAssignableFrom(t) && t != typeof(TMarker));

            foreach (var serviceType in serviceTypes)
            {
                var implementationType = assembly.GetTypes()
                    .FirstOrDefault(c =>
                        c.IsClass &&
                        !c.IsAbstract &&
                        serviceType.IsAssignableFrom(c));

                if (implementationType != null)
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(serviceType, implementationType);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(serviceType, implementationType);
                            break;
                        default:
                            services.AddScoped(serviceType, implementationType);
                            break;
                    }
                }
            }
        }
    }
}