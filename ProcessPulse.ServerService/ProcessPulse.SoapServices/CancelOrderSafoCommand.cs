using Microsoft.Extensions.Options;
using OSB;
using ProcessPulse.ServerService.ProcessPulse.Dbcontext;
using System;
using System.Threading.Tasks;

namespace ProcessPulse.ServerService.ProcessPulse.SoapServices
{
    public class CancelOrderSafoCommand
    {
        private readonly ptSalesOrderFulfillmentPLSPortTypeClient _safoClient;
        private readonly SafoDbContext _dbContext;
        private readonly string _numerZamowienia;

        public CancelOrderSafoCommand(
            ptSalesOrderFulfillmentPLSPortTypeClient safoClient,
            SafoDbContext dbContext,
            IOptions<OrderSettings> orderSettings)
        {
            _safoClient = safoClient;
            _dbContext = dbContext;
            _numerZamowienia = orderSettings.Value.DefaultOrderNumber;
        }

        public async Task<bool> ExecuteAsync(string numerZamowienia)
        {
            try
            {
                var requestHeaderEBM = CreateRequestHeaderEBM();
                var cancelRequest = PrepareCancelOrderRequest(numerZamowienia);

                var response = await _safoClient.opCancelOrderAsync(requestHeaderEBM, cancelRequest);
                return response != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił wyjątek: {ex.Message}");
                return false;
            }
        }

        private RequestHeaderEBMType CreateRequestHeaderEBM()
        {
            return new RequestHeaderEBMType
            {
                RequestID = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                TimestampSpecified = true,
                AuditRecord = new AuditRecordType
                {
                    SystemID = "FLEET",
                    UserID = "fleet",
                    CountryID = "PL_FLEET"
                }
            };
        }

        public CancelOrderFulfillmentRequestEBM PrepareCancelOrderRequest(string numerZamowienia)
        {
            return new CancelOrderFulfillmentRequestEBM
            {
                SalesOrderEBO = new CancelOrderFulfillmentRequestEBMSalesOrderEBO
                {
                    OriginalSalesOrderReference = new ReferenceType
                    {
                        Identification = new IdentificationType
                        {
                            AlternateID = new[] { new OSB.ValueType() { contextName = "FLEET_CARTID", Value = "12345" } }
                        }
                    }
                }
            };
        }
    }
}

