using Microsoft.Extensions.Logging;
using MicrosoftEntraIdDemoApp.Logic.Extensions;
using MicrosoftEntraIdDemoApp.Logic.Models.Configurations;

namespace MicrosoftEntraIdDemoApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            AzureEntraIdConfig config = new("clientId", "tenantId", $"msauth://com.companyname.yourapp/YOUR_HASH")
            {
                ClientId = "xxx",
                TenantId = "xxx",
                RedirectUri = "xxx",
            };

            builder.Services
                .ConfigureAzureEntraId(config)
                .AddHttpServices()
                .AddViewModels()
                .AddViews();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
