using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.SSHCertificates;
using OSB;
using ProcessPulse.ServerService.ProcessPulse.Dbcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        SafoDbContext dbContext)
        {
            _safoClient = safoClient;
            _dbContext = dbContext;
        }


        public async Task ExecuteAsync(string numerZamowienia)
        {
            try
            {
                var requestHeaderEBM = new OSB.RequestHeaderEBMType();
                CancelOrderFulfillmentRequestEBM request = PrepareRequest(numerZamowienia);
                var response = await _safoClient.opCancelOrderAsync(requestHeaderEBM, request);
               
            }
            catch (Exception e)
            {
  
                throw;
            }
        }

        private CancelOrderFulfillmentRequestEBM PrepareRequest(string numerZamowienia)
        {
            var salesOrderEBO = new CancelOrderFulfillmentRequestEBMSalesOrderEBO
            {
                Identification = new IdentificationType
                {
                    BusinessID = new OSB.ValueType
                    {
                        contextName = "PL_SAFO_ORDERID",
                        Value = numerZamowienia
                    }
                }
            };

            return new CancelOrderFulfillmentRequestEBM
            {
                SalesOrderEBO = salesOrderEBO,
                CancelRelatedPurchaseOrderLine = BasicMarkLOVType.T,
                CancelRelatedPurchaseOrderLineSpecified = true
            };
        }
    }
    public class OrderSettings
    {
        public string DefaultOrderNumber { get; set; }
    }
}




