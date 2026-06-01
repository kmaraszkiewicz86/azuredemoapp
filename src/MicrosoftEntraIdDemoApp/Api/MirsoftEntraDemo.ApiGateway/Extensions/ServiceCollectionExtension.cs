using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using System.Text;

namespace MirsoftEntraDemo.ApiGateway.Extensions
{
    public static class ServiceCollectionExtension
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection ConfigureCors()
            {
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        policy.SetIsOriginAllowed(_ => true) //this is only demo purpose, in production you should specify allowed origins!
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
                });

                return services;
            }

            public IServiceCollection ConfigureAzureEntraId(bool isDevelopment, ConfigurationManager configuration)
            {
                // 1. Get the standard AuthenticationBuilder receiver
                var authBuilder = services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                });

                // 2. Configure Web / BFF using the builder instance
                authBuilder.AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"), CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);

                // 3. Configure .NET MAUI Bearer authentication using the same builder instance
                authBuilder.AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"), JwtBearerDefaults.AuthenticationScheme);

                services.AddAuthorization(options =>
                {
                    // Explicitly add both authentication schemes to each custom policy
                    options.AddPolicy("Testowa", policy => policy
                        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme)
                        .RequireClaim("groups", "9b7d25da-24a0-41fc-9fb7-c58cf60a167a"));

                    options.AddPolicy("App-Testers", policy => policy
                        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme)
                        .RequireClaim("groups", "7d8c1230-8e85-4421-8f07-4eb9dcae7812"));

                    options.AddPolicy("Admin", policy => policy
                        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme)
                        .RequireClaim("groups", "b3ed6fe5-0ea5-4be4-8d5f-91c4bd247cfd"));

                    // By default, require authentication for all requests and allow both cookie and bearer token authentication schemes
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme)
                        .Build();
                });

                // For AJAX / BFF calls we should return 401/403 instead of redirecting to the identity provider.
                // Redirects cause the browser to follow a cross-origin redirect and CORS will block the response
                // because the redirect target (login.microsoftonline.com) doesn't contain CORS headers.
                services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    // Make sure auth cookie can be sent in cross-site contexts from the SPA
                    options.Cookie.SameSite = SameSiteMode.None;
                    // In development the SPA may run on http://localhost:4200 — use SameAsRequest so the cookie
                    // is usable during local development. In production keep Always (Secure).
                    if (isDevelopment)
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

                services.Configure<JwtBearerOptions>(
                    JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                var logger = context.HttpContext.RequestServices
                                    .GetRequiredService<ILogger<Program>>();

                                logger.LogInformation("=== AUTH FAILED ===");
                                logger.LogInformation(context.Exception?.ToString() ?? string.Empty);

                                return Task.CompletedTask;
                            },

                            OnTokenValidated = context =>
                            {
                                var logger = context.HttpContext.RequestServices
                                    .GetRequiredService<ILogger<Program>>();

                                StringBuilder claimsStringBuilder = new();

                                logger.LogInformation("=== TOKEN OK ===");

                                var claims = context.Principal?.Claims
                                    .Select(c => $"{c.Type} = {c.Value}");

                                foreach (var claim in claims ?? [])
                                {
                                    claimsStringBuilder.AppendFormat( "{0}; ", claim);
                                }

                                if (claimsStringBuilder.Length > 0)
                                {
                                    logger.LogInformation(claimsStringBuilder.ToString());
                                }

                                return Task.CompletedTask;
                            },

                            OnChallenge = context =>
                            {
                                var logger = context.HttpContext.RequestServices
                                    .GetRequiredService<ILogger<Program>>();

                                logger.LogInformation("=== CHALLENGE ===");
                                logger.LogInformation(context.Error);
                                logger.LogInformation(context.ErrorDescription);

                                return Task.CompletedTask;
                            }
                        };
                    });

                return services;
            }
        }
    }
}
