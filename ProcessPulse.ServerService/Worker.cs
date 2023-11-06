using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ProcessPulse.Class.ProcessPulse.Models; // Assuming this is where your ProcessInfoService is located
using Microsoft.EntityFrameworkCore;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
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

                // Odczekanie pewnego czasu przed kolejn¹ iteracj¹
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }
    }
}