using Microsoft.Identity.Client;
using MicrosoftEntraIdDemoApp.Logic.Features;
using MicrosoftEntraIdDemoApp.Logic.Features.AuthTest;
using MicrosoftEntraIdDemoApp.Logic.Features.Login;
using MicrosoftEntraIdDemoApp.Logic.Features.UserCheck;
using MicrosoftEntraIdDemoApp.Logic.Models.Configurations;

namespace MicrosoftEntraIdDemoApp.Logic.Extensions
{
    public static class HttpResponseExtension
    {
        extension(HttpResponseMessage response)
        {
            /// <summary>
            /// Extension method to map HTTP status codes to human-readable messages
            /// </summary>
            /// <returns></returns>
            public string ToUserFriendlyMessage()
            {
                return response.StatusCode switch
                {
                    System.Net.HttpStatusCode.Unauthorized => "User not authenticated. Please login again.", // 401
                    System.Net.HttpStatusCode.Forbidden => "You don't have permission to access this resource.", // 403
                    System.Net.HttpStatusCode.NotFound => "The requested resource was not found.", // 404
                    System.Net.HttpStatusCode.InternalServerError => "Server error. Please try again later.", // 500
                    _ => $"Unexpected error: {response.ReasonPhrase} ({(int)response.StatusCode})"
                };
            }
        }
    }

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

                return services;
            }
        }
    }

}
