using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProcessPulse.BibliotekaKlas.ProcessPulse.Models;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService() // Konfiguracja aplikacji do uruchomienia jako us³uga Windows
            .ConfigureServices((hostContext, services) =>
            {
                // Rejestrowanie DbContext
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                // Rejestrowanie ProcessInfoService jako us³ugi o czasie ¿ycia 'scoped'
                services.AddScoped<ProcessInfoService>();

                // Rejestrowanie Worker jako us³ugi hostowanej, która dzia³a jako singleton
                services.AddHostedService<Worker>();

                // Konfiguracja logowania, jeœli potrzebne
                services.AddLogging(configure => configure.AddConsole());
            });
}

