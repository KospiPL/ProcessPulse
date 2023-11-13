using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ProcessPulse.ServerService.ProcessPulse.Service;
using Oracle.EntityFrameworkCore; 
using ProcessPulse.Class.ProcessPulse.Models;
using ProcessPulse.ServerService.ProcessPulse.Dbcontext;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureServices((hostContext, services) =>
            {
                // Konfiguracja kontekstów DbContext
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));
                services.AddDbContext<FlotaDbContext>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("FlotaDatabase")));
                services.AddDbContext<SafoDbContext>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("SafoDatabase")));

                // Rejestracja us³ug
                services.AddScoped<ProcessInfoService>();
                services.AddHostedService<Worker>();
                services.AddHttpClient();
                services.AddScoped<FlotaService>(provider =>
                    new FlotaService(
                        provider.GetRequiredService<FlotaDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("OracleConnection") 
                    ));
                services.AddScoped<SafoService>();

                // Rejestracja logowania
                services.AddLogging(configure => configure.AddConsole());
            });
}