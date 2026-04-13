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
.AddMicrosoftIdentityWebApp(options =>
{
    builder.Configuration.GetSection("AzureAd").Bind(options);
    options.TokenValidationParameters.RoleClaimType = "groups";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Testowa", policy => policy.RequireClaim("groups", "9b7d25da-24a0-41fc-9fb7-c58cf60a167a"));
    options.AddPolicy("App-Testers", policy => policy.RequireClaim("groups", "7d8c1230-8e85-4421-8f07-4eb9dcae7812"));
    options.AddPolicy("Admin", policy => policy.RequireClaim("groups", "b3ed6fe5-0ea5-4be4-8d5f-91c4bd247cfd"));
});

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
        options.Cookie.Domain = ".azurewebsites.net";
    }
    options.Cookie.HttpOnly = true;

    options.Events.OnRedirectToLogin = async context =>
    {
        var path = context.Request.Path;
        if (path.StartsWithSegments("/bff") || path.StartsWithSegments("/api") || context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
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
        if (path.StartsWithSegments("/bff") || path.StartsWithSegments("/api") || context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
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

app.MapGet("/bff/user", (ClaimsPrincipal user) =>
{
    var roles = user
        .Claims
        .Where(c => c.Type == "groups" )
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
    // Use your existing logic to get group names
    var userGroups = user.Claims
        .Where(c => c.Type == "groups")
        .Select(c => c.GetGoupName()) // Assuming this extension method maps ID to Name
        .ToArray();

    StringBuilder messages = new ();

    // Check if the mapped names exist in the array
    if (userGroups.Contains("Testowa"))
    {
        messages.AppendLine("You are in the Testowa group");
    }

    if (userGroups.Contains("App-Testers"))
    {
        messages.AppendLine("You are in the App-Testers group");
    }

    if (userGroups.Contains("SuperDruperAdminZDuper"))
    {
        messages.AppendLine("You are in the SuperDruperAdminZDuper group");
    }

    if (messages.Length > 0)
    {
        return Results.Ok(new { Message = messages.ToString() });
    }

    return Results.Ok(new { Message = "You don't have any group" });
})
.RequireAuthorization();

// Endpoint restricted to App-Testers group members only
app.MapGet("/api/apptesters", () => {
    return new { Message = "You are in the App-Testers group" };
}).RequireAuthorization(policy => policy.RequireRole("App-Testers"));

// Endpoint restricted to App-Testers group members only
app.MapGet("/api/test", () => {
    return new { Message = "You are in the Testowa group!" };
}).RequireAuthorization("Testowa");

// Endpoint restricted to App-Testers group members only
app.MapGet("/api/admin", () => {
    return new { Message = "You are in the SuperDruperAdminZDuper group!" };
}).RequireAuthorization("Admin");

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
