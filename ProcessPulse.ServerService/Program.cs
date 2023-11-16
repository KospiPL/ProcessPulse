using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ProcessPulse.ServerService.ProcessPulse.Service;
using Oracle.EntityFrameworkCore;
using ProcessPulse.Class.ProcessPulse.Models;
using ProcessPulse.ServerService.ProcessPulse.Dbcontext;
using System.ServiceModel;
using OSB;
using System.Net;
using ProcessPulse.ServerService.ProcessPulse.SoapServices;
using System.Linq;
using System.Configuration;

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
                services.AddScoped<CancelOrderSafoCommand>();

                services.AddScoped<FlotaService>(provider =>
                    new FlotaService(
                        provider.GetRequiredService<FlotaDbContext>(),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("OracleConnection") 
                    ));
                services.AddScoped<ptSalesOrderFulfillmentPLSPortTypeClient>(provider =>
                {
                    // Tutaj skonfiguruj i stwórz instancjê klienta SOAP
                    var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                    var endpointAddress = new EndpointAddress("https://osbicc01t.intercars.local:1116/SOALibrary/EnterpriseServiceLibrary/ProcessLevel/Service/SalesOrderFulfillment/V2");
                    var client = new ptSalesOrderFulfillmentPLSPortTypeClient(binding, endpointAddress);

                    // Konfiguracja nag³ówków bezpieczeñstwa (np. IcSecurityHeader)
                    client.Endpoint.RegisterIcSecurity(); 

                    // Konfiguracja dodatkowych zachowañ endpointa
                    client.Endpoint.RegisterRequestHeader("PL_FLEET");
                    client.Endpoint.RegisterMaxFaultSize(2097152);
                    client.ClientCredentials.UserName.UserName = "fleet";
                    client.ClientCredentials.UserName.Password = "fleet#abc123";

                    // Usuwanie niepotrzebnych zachowañ
                    var vs = client.Endpoint.EndpointBehaviors.FirstOrDefault(i => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
                    if (vs != null)
                    {
                        client.Endpoint.EndpointBehaviors.Remove(vs);
                    }

                    return client;
                });
                services.AddScoped<SafoService>();

                // Rejestracja logowania
                services.AddLogging(configure => configure.AddConsole());
            });
}