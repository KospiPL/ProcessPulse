using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;

namespace ProcessPulse.ServerService.ProcessPulse.SoapServices
{
    public static class EndpointBehaviorExtensions
    {
        public static void RegisterIcSecurity(this ServiceEndpoint endpoint)
        {
            endpoint.EndpointBehaviors.Add(new IcSecurityBehavior());
        }

        public static void RegisterRequestHeader(this ServiceEndpoint endpoint, string systemId)
        {
            endpoint.EndpointBehaviors.Add(new RequestHeaderBehavior(systemId));
        }

        public static void RegisterMaxFaultSize(this ServiceEndpoint endpoint, int size)
        {
            endpoint.EndpointBehaviors.Add(new MaxFaultSizeBehavior(size));
        }
    }
}
