using System;
using System.ServiceModel;
using System.Threading.Tasks;
using ProcessPulse.ServerService.ProcessPulse.Dbcontext;
using ProcessPulse.Class.ProcessPulse.Models;
using OSB;
using ProcessPulse.ServerService.ProcessPulse.SoapServices;
using System.ServiceModel.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class SafoService
{
    private readonly ptSalesOrderFulfillmentPLSPortTypeClient _safoClient;
    private readonly ptSalesOrderFulfillmentPLSPortTypeClient _navClient;
    private readonly SafoDbContext _dbContext;
    private readonly IOptions<OrderSettings> _orderSettings;

    public SafoService(ptSalesOrderFulfillmentPLSPortTypeClient safoClient,
                       ptSalesOrderFulfillmentPLSPortTypeClient navClient,
                       SafoDbContext dbContext,
                       IOptions<OrderSettings> orderSettings)
    {
        _safoClient = safoClient;
        _navClient = navClient;
        _dbContext = dbContext;
        _orderSettings = orderSettings;
    }

    public async Task CheckAndCancelOrderAsync(string numerZamowienia = null)
    {
        numerZamowienia ??= _orderSettings.Value.DefaultOrderNumber;

        try
        {
            var responseSafo = await CheckServiceAvailabilityAsync(_safoClient, "PL");
            var responseNav = await CheckServiceAvailabilityAsync(_navClient, "PL_FLEET");

            LogToDatabase(responseSafo, responseNav);

            if (responseSafo && responseNav)
            {
                await CancelOrderAsync(numerZamowienia);
            }
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Wystąpił wyjątek: {ex.Message}");
        }
    }

    private async Task CancelOrderAsync(string numerZamowienia)
    {
        var cancelCommand = new CancelOrderSafoCommand(_safoClient, _dbContext, _orderSettings);
        await cancelCommand.ExecuteAsync(numerZamowienia);
    }
    private CancelOrderFulfillmentRequestEBM PrepareCancelOrderRequest()
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
    private async Task<bool> CheckServiceAvailabilityAsync(ptSalesOrderFulfillmentPLSPortTypeClient client, string systemId)
    {
        try
        {
            var requestHeaderEBM = new RequestHeaderEBMType();
            var cancelRequest = PrepareCancelOrderRequest();

            using (new OperationContextScope(client.InnerChannel))
            {
                var icSecurityHeader = new IcSecurityHeader();
                var customHeader = new RequestHeader(systemId);

                OperationContext.Current.OutgoingMessageHeaders.Add(icSecurityHeader);
                OperationContext.Current.OutgoingMessageHeaders.Add(customHeader);

                var response = await client.opCancelOrderAsync(null, cancelRequest);

                if (response == null)
                {
                    return false;
                }

              
                return response.ResponseHeaderEBM?.ResponseID?.StartsWith("OSB") ?? false;
            }
        }
        catch (FaultException<OSB.FaultMessageEBMType> faultEx)
        {
    
            if (faultEx.Detail != null && faultEx.Detail.ToString().StartsWith("OSB"))
            {          
                return true;
            }
            else
            {         
                Console.WriteLine($"Wystąpił wyjątek: {faultEx.Message}");
             
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił nieoczekiwany wyjątek: {ex.Message}");
            return false;
        }
    }



    private void LogToDatabase(bool isSafoConnected, bool isNavConnected)
    {
        var log = new SafoModel
        {
            IsConnectedSafo = isSafoConnected,
            IsConnectedNav = isNavConnected,
            Data = DateTime.Now
        };

        _dbContext.SafoModels.Add(log);
        _dbContext.SaveChanges();
    }
}
public class OrderSettings
{
    public string DefaultOrderNumber { get; set; }
}   