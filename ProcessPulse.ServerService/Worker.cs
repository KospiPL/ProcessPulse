using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ProcessPulse.BibliotekaKlas.ProcessPulse.Models; // Assuming this is where your ProcessInfoService is located
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
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    // Retrieve the machine name
                    var machineName = Environment.MachineName;

                    // Get the process name for the machine from the TerminalMapping table
                    var terminalMapping = await dbContext.TerminalMapping
                        .FirstOrDefaultAsync(t => t.TerminalId == machineName, stoppingToken);

                    if (terminalMapping != null)
                    {
                        // Get process information based on the process name
                        var processInfos = await processInfoService.GetProcessNameByHostNameAsync(terminalMapping.Name);
                       

                       
                        _logger.LogInformation($"Successfully retrieved and saved process info for {machineName} at {DateTime.UtcNow}");
                    }
                    else
                    {
                        _logger.LogWarning($"No terminal mapping found for machine name: {machineName}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while retrieving and saving process info.");
                }
            }

            // Wait some time before running the loop again
            await Task.Delay(60000, stoppingToken); // waits for 60 seconds
        }
    }
}