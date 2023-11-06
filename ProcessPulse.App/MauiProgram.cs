using Blazorise;
using Blazorise.Bootstrap5;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProcessPulse.App.Data;
using ProcessPulse.App.Services;
using ProcessPulse.Class.ProcessPulse.Models;
using ProcessPulse.Class.Service;
using Blazorise.Icons.FontAwesome;


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

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazorise(options =>
                            {
                             options.Immediate = true;
                             })
                             .AddBootstrap5Providers()
                             .AddFontAwesomeIcons();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                // Dla SQL Server
                options.UseSqlServer("Data Source=kacpercudzik.database.windows.net;Initial Catalog=ProcessPulse;Persist Security Info=True;User ID=Kacpercudzik;Password=AChiscHeDEnEl#");
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped< ITerminalService , TerminalService > ();
            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}
