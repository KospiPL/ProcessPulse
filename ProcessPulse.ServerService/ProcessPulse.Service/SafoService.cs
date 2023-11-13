using ProcessPulse.ServerService.ProcessPulse.Dbcontext;
using ProcessPulse.Class.ProcessPulse.Models;
using System.Threading.Tasks;
using ProcessPulse.ServerService.ProcessPulse.SoapServices; 
using Azure;
using OSB;

namespace ProcessPulse.ServerService.ProcessPulse.Service
{
    public class SafoService
    {
        private readonly SafoDbContext _safoContext;
        private readonly ptSalesOrderFulfillmentPLSPortTypeClient _soapClient;

        public SafoService(SafoDbContext safoContext, ptSalesOrderFulfillmentPLSPortTypeClient soapClient)
        {
            _safoContext = safoContext;
            _soapClient = soapClient;
        }

        private async Task<bool> CheckConnectionAsync(string systemId)
        {
            try
            {
                await _soapClient.OpenAsync(systemId); // Zakładamy, że OpenAsync to metoda Twojego klienta SOAP
                return true; // Jeśli odpowiedź jest nienullowa, zakładamy, że połączenie jest otwarte
            }
            catch
            {
                return false; // W przypadku wyjątku zakładamy, że połączenie nie jest dostępne
            }
        }

        public async Task CheckAndStoreSafoConnectionStatus()
        {
            var isConnectedSafo = await CheckConnectionAsync("PL");
            var isConnectedNav = await CheckConnectionAsync("PL_FLEET");

            var safoRecord = new SafoModel { IsConnectedSafo = isConnectedSafo, IsConnectedNav = isConnectedNav };
            _safoContext.SafoModels.Add(safoRecord);
            await _safoContext.SaveChangesAsync();
        }
    }
}