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
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));
                services.AddDbContext<FlotaDbContext>(options =>
                    options.UseOracle(hostContext.Configuration.GetConnectionString("FlotaDatabase")));

                services.AddScoped<ProcessInfoService>();
                services.AddHostedService<Worker>();

                services.AddScoped<SafoDatabaseService>();
                services.AddHttpClient<OsbService>();
                services.AddHostedService<ServerStatusService>();

                services.AddLogging(configure => configure.AddConsole());
            });
}