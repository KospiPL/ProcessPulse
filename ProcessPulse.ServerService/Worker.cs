using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ProcessPulse.ServerService.ProcessPulse.Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSpan _flotaSafoInterval = TimeSpan.FromMinutes(5);
    private readonly TimeSpan _processInfoInterval = TimeSpan.FromSeconds(15);
    private DateTime _lastFlotaSafoRun = DateTime.MinValue;
    private DateTime _lastProcessInfoRun = DateTime.MinValue;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var currentTime = DateTime.UtcNow;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                // Wykonywanie zadañ ProcessInfoService co 15 sekund
                if (currentTime - _lastProcessInfoRun >= _processInfoInterval)
                {
                    await ExecuteProcessInfoTasks(scope);
                    _lastProcessInfoRun = currentTime;
                }

                // Wykonywanie zadañ FlotaService i SafoService co 5 minut
                if (currentTime - _lastFlotaSafoRun >= _flotaSafoInterval)
                {
                    await ExecuteFlotaSafoTasks(scope);
                    _lastFlotaSafoRun = currentTime;
                }
            }

            // Odczekanie krótkiego czasu przed kolejn¹ iteracj¹
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }

    private async Task ExecuteProcessInfoTasks(IServiceScope scope)
    {
        var processInfoService = scope.ServiceProvider.GetRequiredService<ProcessInfoService>();

                try
                {
                    // Pobranie nazwy maszyny
                    string hostName = Environment.MachineName;

            // Pobranie nazwy procesu na podstawie nazwy maszyny
            string processName = await processInfoService.GetProcessNameByHostNameAsync(hostName);
            if (!string.IsNullOrEmpty(processName))
            {
                // Pobranie danych o procesie i zapisanie ich w bazie danych
                await processInfoService.GetProcessResourceDataByNameAsync(processName);
                _logger.LogInformation($"Dane procesu dla {hostName} ({processName}) zaktualizowane.");
            }
            else
            {
                _logger.LogWarning($"Nie znaleziono mapowania dla maszyny o nazwie: {hostName}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Wyst¹pi³ wyj¹tek podczas zbierania danych o procesie: {ex}");
        }
    }

    private async Task ExecuteFlotaSafoTasks(IServiceScope scope)
    {
        var flotaService = scope.ServiceProvider.GetRequiredService<FlotaService>();
        var safoService = scope.ServiceProvider.GetRequiredService<SafoService>();

        try
        {
            await flotaService.CheckAndStoreConnectionStatus();
            await safoService.CheckAndStoreSafoConnectionStatus();
            _logger.LogInformation("Dane Floty i SAFO zosta³y zaktualizowane");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Wyst¹pi³ wyj¹tek podczas aktualizacji danych Floty i SAFO: {ex}");
        }
    }
}