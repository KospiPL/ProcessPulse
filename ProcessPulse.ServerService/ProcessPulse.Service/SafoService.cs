using Microsoft.EntityFrameworkCore;
using ProcessPulse.Class.ProcessPulse.Models;
using ProcessPulse.ServerService.ProcessPulse.Dbcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProcessPulse.ServerService.ProcessPulse.Service
{
    public class SafoService
    {
        private readonly HttpClient _httpClient;
        private readonly SafoDbContext _safoContext;

        public SafoService(IHttpClientFactory httpClientFactory, SafoDbContext safoContext)
        {
            _httpClient = httpClientFactory.CreateClient();
            _safoContext = safoContext;
        }

        public async Task CheckAndStoreSafoConnectionStatus()
        {
            var isConnectedSafo = await CheckConnectionAsync("https://osbicc01t.intercars.local:1116/SOALibrary/EnterpriseServiceLibrary/ProcessLevel/Service/SalesOrderFulfillment/V2", "PL");
            var isConnectedNav = await CheckConnectionAsync("https://osbicc01t.intercars.local:1116/SOALibrary/EnterpriseServiceLibrary/ProcessLevel/Service/SalesOrderFulfillment/V2", "PL_FLEET");

            var safoRecord = new SafoModel { IsConnectedSafo = isConnectedSafo, IsConnectedNav = isConnectedNav };
            _safoContext.SafoModels.Add(safoRecord);
            await _safoContext.SaveChangesAsync();
        }

        private async Task<bool> CheckConnectionAsync(string url, string systemId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{url}?systemId={systemId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}