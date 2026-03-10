using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using MirsoftEntraDemo.Api.Features;
using SimpleCqrs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Konfiguracja SimpleCqrs
builder.Services.ConfigureSimpleCqrs(typeof(Program).Assembly);

// Konfiguracja Microsoft Entra ID
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "MirsoftEntraDemo API");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Rejestracja wszystkich feature endpoints
app.MapFeatureEndpoints();

app.Run();
