using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPulse.ServerService.ProcessPulse.Service
{
    public class ServerStatusService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                CheckServerStatus();
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private void CheckServerStatus()
        {
            // Logika sprawdzania statusu serwera
        }
    }

}
