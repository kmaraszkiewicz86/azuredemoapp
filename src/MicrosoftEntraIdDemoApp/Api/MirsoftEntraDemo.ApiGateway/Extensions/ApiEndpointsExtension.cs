using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;
using System.Text;

namespace MirsoftEntraDemo.ApiGateway.Extensions
{
    public static class ApiEndpointsExtension
    {
        extension(WebApplication app)
        {
            public WebApplication MapEndpoints()
            {
                app.MapGet("/login", (string? redirect) => Results.Challenge(
                    new AuthenticationProperties { RedirectUri = redirect },
                    [OpenIdConnectDefaults.AuthenticationScheme]
                ));

                app.MapGet("/bff/user", (ClaimsPrincipal user) =>
                {
                    var roles = user
                        .Claims
                        .Where(c => c.Type == "groups")
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

                    StringBuilder messages = new();

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
                app.MapGet("/api/apptesters", () =>
                {
                    return new { Message = "You are in the App-Testers group" };
                }).RequireAuthorization(policy => policy.RequireRole("App-Testers"));

                // Endpoint restricted to App-Testers group members only
                app.MapGet("/api/test", () =>
                {
                    return new { Message = "You are in the Testowa group!" };
                }).RequireAuthorization("Testowa");

                // Endpoint restricted to App-Testers group members only
                app.MapGet("/api/admin", () =>
                {
                    return new { Message = "You are in the SuperDruperAdminZDuper group!" };
                }).RequireAuthorization("Admin");

                app.MapGet("/logout", (string? redirect) => Results.SignOut(
                            new AuthenticationProperties { RedirectUri = redirect },
                            [
                                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme
                            ]
                    ));

                return app;
            }
        }
    }
}
