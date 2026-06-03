using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Abstractions;
using MicrosoftEntraIdDemoApp.Logic.Features;
using MicrosoftEntraIdDemoApp.Logic.Features.AuthTest;
using MicrosoftEntraIdDemoApp.Logic.Features.Login;
using MicrosoftEntraIdDemoApp.Logic.Features.UserCheck;
using MicrosoftEntraIdDemoApp.Logic.Models.Configurations;
using MicrosoftEntraIdDemoApp.Logic.Shared;
using MicrosoftEntraIdDemoApp.Logic.Shared.Security;
using MicrosoftEntraIdDemoApp.Logic.ViewModels;
using System.Diagnostics;

namespace MicrosoftEntraIdDemoApp.Logic.Extensions
{
    public static class ServiceCollectionExtension
    {
        extension (IServiceCollection services)
        {
            public IServiceCollection ConfigureAzureEntraId()
            {
                services.AddTransient<IPublicClientApplication>(opt =>
                {
                    PublicClientApplicationBuilder builder = PublicClientApplicationBuilder
                        .Create(AzureEntraIdConfig.ClientId)
                        .WithAuthority(AzureEntraIdConfig.Authority)
                        .WithLogging(new IdentityLogger(EventLogLevel.Warning), enablePiiLogging: false)
                        .WithRedirectUri($"msal{AzureEntraIdConfig.ClientId}://auth");

                    return builder.Build();
                });

                services.AddSingleton<ITokenService, TokenService>();
                services.AddTransient<ILoginHttpService, LoginHttpService>();


                return services;
            }

            public IServiceCollection AddViews()
            {
                services.AddTransient<INavigationService, NavigationService>();

                services.AddTransient<LoginPage>();
                services.AddTransient<UserCheckPage>();
                services.AddTransient<AuthTestPage>();

                return services;
            }

            public IServiceCollection AddViewModels()
            {
                services.AddTransient<LoginViewModel>();
                services.AddTransient<UserCheckViewModel>();
                services.AddTransient<AuthTestViewModel>();
                services.AddSingleton<AppShellViewModel>();

                return services;
            }

            public IServiceCollection AddHttpServices()
            {
                var baseAddress = new Uri("https://app-entra-demo-api-gateway-cff7bjdec5dxahe3.canadacentral-01.azurewebsites.net/");

                services.AddHttpClient<IAuthTestHttpService, AuthTestHttpService>(client =>
                {
                    // Set the base URL for this specific client so you don't repeat it in the service
                    client.BaseAddress = baseAddress;

                    // Optional: Set default timeouts or headers here (e.g., standard Accept headers)
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });

                services.AddHttpClient<IUserCheckHttpService, UserCheckHttpService>(client =>
                {
                    // Set the base URL for this specific client so you don't repeat it in the service
                    client.BaseAddress = baseAddress;

                    // Optional: Set default timeouts or headers here (e.g., standard Accept headers)
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });

                return services;
            }

            public IServiceCollection AddSharedServices()
            {
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<ITokenService, TokenService>();

                return services;
            }
        }
    }

}
