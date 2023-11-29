using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using GC.Auth.Permissions;
using GearCode.Common.Auth.Forms;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using ProcessPulse.App.Data;
using ProcessPulse.App.Services;
using ProcessPulse.Class.ProcessPulse.Models;
using ProcessPulse.Class.Service;

namespace ProcessPulse.App
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
                });

            // Inicjalizacja IdentityManager
            InitializeIdentityManager();

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazorise(options =>
            {
                options.Immediate = true;
            })
            .AddBootstrap5Providers()
            .AddFontAwesomeIcons();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped<ITerminalService, TerminalService>();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthState>();

            return builder.Build();
        }

        private static void InitializeIdentityManager()
        {
           
            var config = new IdentityManagerConfig(
                title: "Auth",
                clientAppCode: "GC.CART", 
                resourceAppCode: "GC.CATALOG.API", 
                languageCode: "pl", 
                environment: AuthEnvironment.Production, 
                sessionPath: null, 
                forcedTenant: null
            );

           
            IdentityManager.Initialize(
                config: config,
                permissionsMapper: new PermissionsMapperByAttributes(typeof(GearCodePermissions)),
                logger: null 
            );

            
        }
    }
}