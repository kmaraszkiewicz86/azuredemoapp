using AzureJsonDataFlowFunction.Extensions;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .RegisterExternalAzureClients(builder.Configuration)
    .RegisterAllServices();

builder.Build().Run();
