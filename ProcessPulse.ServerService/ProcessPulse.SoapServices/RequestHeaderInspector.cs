using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using VaultSharp.V1.SystemBackend;

namespace ProcessPulse.ServerService.ProcessPulse.SoapServices
{
    internal class RequestHeaderInspector : IClientMessageInspector
    {
        private readonly string _systemId;

        public RequestHeaderInspector(string systemId)
        {
            _systemId = systemId;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var rh = new RequestHeader(_systemId);
            request.Headers.Add(rh);
            return rh;
        }
    }
}
