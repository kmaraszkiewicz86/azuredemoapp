using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddSingleton(x =>
{
    var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
    return new BlobServiceClient(connectionString);
});

builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    string endpointUri = configuration["CosmosDb:Endpoint"]!;
    string primaryKey = configuration["CosmosDb:PrimaryKey"]!;

    return new CosmosClient(endpointUri, primaryKey);
});

builder.Build().Run();
