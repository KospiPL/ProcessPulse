using OSB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using ProcessPulse.ServerService.ProcessPulse.SoapServices;

namespace ProcessPulse.ServerService.ProcessPulse.SoapServices
{
    public class SoapClientFactory
    {
        private const string UriSalesOrderFulfillment = "https://osbicc01t.intercars.local:1116/SOALibrary/EnterpriseServiceLibrary/ProcessLevel/Service/SalesOrderFulfillment/V2";
        public static ptSalesOrderFulfillmentPLSPortTypeClient CreateSalesOrderFulfillmentClient(string systemId)
        {
            ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;

            var serviceUri = new Uri(UriSalesOrderFulfillment);
            var endPoint = new EndpointAddress(serviceUri);

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            var client = new ptSalesOrderFulfillmentPLSPortTypeClient(binding, endPoint);

            client.Endpoint.RegisterIcSecurity();
            client.Endpoint.RegisterRequestHeader(systemId);
            client.Endpoint.RegisterMaxFaultSize(2097152);
            var vs = client.Endpoint.EndpointBehaviors.FirstOrDefault((i) => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
            if (vs != null)
            {
                client.Endpoint.EndpointBehaviors.Remove(vs);
            }

            return client;
        }
    }
}
