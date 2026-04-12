using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using MirsoftEntraDemo.ApiGateway.Extensions;
using System.Security.Claims;
using System.Text;

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
builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    // Make sure auth cookie can be sent in cross-site contexts from the SPA
    options.Cookie.SameSite = SameSiteMode.None;
    // In development the SPA may run on http://localhost:4200 — use SameAsRequest so the cookie
    // is usable during local development. In production keep Always (Secure).
    if (builder.Environment.IsDevelopment())
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    }
    else
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    }
    options.Cookie.HttpOnly = true;

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

app.MapGet("/login", (string? redirect) => Results.Challenge(
        new AuthenticationProperties { RedirectUri = redirect },
        [OpenIdConnectDefaults.AuthenticationScheme]
    ));

app.MapPost("/bff/user", (ClaimsPrincipal user) =>
{


    var roles = user
        .Claims
        .Where(c => c.Type == ClaimTypes.Role || c.Type == "roles" || c.Type == "groups" )
        .Select(c => c.GetGoupName())
        .ToArray();

    return new
    {
        user.Identity?.Name,
        roles
    };
}).RequireAuthorization();

app.MapGet("/api/checkGroup", (ClaimsPrincipal user) =>
{
    StringBuilder messages = new ();

    if (user.IsInRole("Testowa"))
    {
        messages.AppendLine("You are in the Testowa group");
    }

    if (user.IsInRole("App-Testers"))
    {
        messages.AppendLine("You are in the App-Testers group");
    }

    if (messages.Length > 0)
    {
        return messages.ToString();
    }

    return "You are not a Testowa user.";
})
.RequireAuthorization();

// Endpoint restricted to App-Testers group members only
app.MapGet("/api/apptesters", () => {
    return "You are in the App-Testers group";
}).RequireAuthorization(policy => policy.RequireRole("App-Testers"));

// Endpoint restricted to App-Testers group members only
app.MapGet("/api/test", () => {
    return "You are in the Testowa group!";
}).RequireAuthorization(policy => policy.RequireRole("Testowa"));

app.MapGet("/logout", (string? redirect) => Results.SignOut(
            new AuthenticationProperties { RedirectUri = redirect },
            [
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme
            ]
    ));

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "MirsoftEntraDemo API Gateway");
});

app.MapReverseProxy();

app.Run();
