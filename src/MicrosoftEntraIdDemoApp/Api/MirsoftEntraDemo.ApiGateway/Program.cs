using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(_ => true) //this is only demo purpose, in production you should specify allowed origins!
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors();

var frontendUrl = $"{app.Configuration["FrontendUrl"]}usercheck"
    ?? throw new InvalidOperationException("The parameter FrontendUrl is empty. Check appsettings.json.");

app.MapGet("/login", () =>
{
    return Results.Challenge(
        new AuthenticationProperties { RedirectUri = frontendUrl },
        [ OpenIdConnectDefaults.AuthenticationScheme ]
    );
});


app.MapPost("/bff/user", (ClaimsPrincipal user) =>
{
    return new
    {
        user.Identity?.Name,
    };
}).RequireAuthorization();

app.MapGet("/logout", () =>
{
    return Results.SignOut(
            new AuthenticationProperties { RedirectUri = frontendUrl },
            [
                CookieAuthenticationDefaults.AuthenticationScheme, 
                OpenIdConnectDefaults.AuthenticationScheme
            ]
        );
});

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "MirsoftEntraDemo API Gateway");
});

app.MapReverseProxy();

app.Run();
