using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

// For AJAX / BFF calls we should return 401/403 instead of redirecting to the identity provider.
// Redirects cause the browser to follow a cross-origin redirect and CORS will block the response
// because the redirect target (login.microsoftonline.com) doesn't contain CORS headers.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = async context =>
    {
        var path = context.Request.Path;
        if (path.StartsWithSegments("/bff") || context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // For non-AJAX requests, trigger OpenID Connect challenge to redirect user to IdP
        await context.HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = context.RedirectUri });
    };

    options.Events.OnRedirectToAccessDenied = async context =>
    {
        var path = context.Request.Path;
        if (path.StartsWithSegments("/bff") || context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        // For non-AJAX requests, trigger the OpenID Connect challenge/forbid flow
        await context.HttpContext.ForbidAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = context.RedirectUri });
    };
});

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

// Ensure authentication/authorization middleware run after CORS so preflight requests are handled
app.UseAuthentication();
app.UseAuthorization();

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
