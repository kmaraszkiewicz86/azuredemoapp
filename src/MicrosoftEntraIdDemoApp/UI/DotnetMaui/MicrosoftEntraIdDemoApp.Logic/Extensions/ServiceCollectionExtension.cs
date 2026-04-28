using Microsoft.Identity.Client;
using MicrosoftEntraIdDemoApp.Logic.Features;
using MicrosoftEntraIdDemoApp.Logic.Features.Login;
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

                return services;
            }

            public IServiceCollection AddViewModels()
            {
                services.AddTransient<LoginViewModel>();

                return services;
            }

            public IServiceCollection AddHttpServices()
            {
                return services;
            }
        }
    }

}
