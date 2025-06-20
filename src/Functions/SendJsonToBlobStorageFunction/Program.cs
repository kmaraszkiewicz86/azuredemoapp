using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddSingleton(x =>
{
    var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
    return new BlobServiceClient(connectionString);
});

builder.Build().Run();
