using MirsoftEntraDemo.ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.ConfigureAzureEntraId(builder.Environment.IsDevelopment(), builder.Configuration);

builder.Services.ConfigureCors();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

WebApplication app = builder.Build();

app.UseCors();

// Ensure authentication/authorization middleware run after CORS so preflight requests are handled
app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "MirsoftEntraDemo API Gateway");
});

app.MapReverseProxy();

app.Run();
