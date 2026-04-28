using Microsoft.Identity.Client;
using MicrosoftEntraIdDemoApp.Logic.Features;
using MicrosoftEntraIdDemoApp.Logic.Features.AuthTest;
using MicrosoftEntraIdDemoApp.Logic.Features.Login;
using MicrosoftEntraIdDemoApp.Logic.Features.UserCheck;
using MicrosoftEntraIdDemoApp.Logic.Models.Configurations;

namespace MicrosoftEntraIdDemoApp.Logic.Extensions
{
    public static class ServiceCollectionExtension
    {
        extension (IServiceCollection services)
        {
            public IServiceCollection ConfigureAzureEntraId(AzureEntraIdConfig azureEntraIdConfig)
            {
                services.AddTransient<IPublicClientApplication>(opt =>
                {
                    PublicClientApplicationBuilder builder = PublicClientApplicationBuilder.Create(azureEntraIdConfig.ClientId)
                        .WithAuthority(AzureCloudInstance.AzurePublic, azureEntraIdConfig.TenantId)
                        // This must match your Android/iOS configuration in Azure
                        .WithRedirectUri(azureEntraIdConfig.RedirectUri)
                        .WithIosKeychainSecurityGroup("com.microsoft.adalcache");
#if ANDROID
                    builder.WithParentActivityOrWindow(() => Platform.CurrentActivity);
#endif

                    return builder.Build();
                });

                services.AddTransient<ILoginHttpService, LoginHttpService>();


                return services;
            }

            public IServiceCollection AddViews()
            {
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

                return services;
            }

            public IServiceCollection AddHttpServices()
            {
                var baseAddress = new Uri("https://app-entra-demo-api-gateway-cff7bjdec5dxahe3.canadacentral-01.azurewebsites.net");

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
        }
    }

}
