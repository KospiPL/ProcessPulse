using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPulse.ServerService.ProcessPulse.SoapServices
{
    internal class IcSecurityInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // Nothing to do here
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var header = new IcSecurityHeader();
            request.Headers.Add(header);
            return header;
        }
    }
}
