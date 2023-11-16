using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Xml;

namespace ProcessPulse.ServerService.ProcessPulse.SoapServices
{
    [DataContract]
    public class RequestHeader : MessageHeader
    {
        [DataMember]
        private string _systemId;  

        
        public RequestHeader()
        {
        }

        
        public RequestHeader(string systemId)
        {
            _systemId = systemId;
        }

        public override string Name => "RequestHeaderEBM";
        public override string Namespace => "http://intercars.com.pl/SOALibrary/EnterpriseObjectLibrary/Common/V1";

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteElementString("RequestID", Guid.NewGuid().ToString());
            writer.WriteElementString("Timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));

            writer.WriteStartElement("AuditRecord", null);
            writer.WriteElementString("SystemID", "FLEET");
            writer.WriteElementString("UserID", "fleet");
            writer.WriteElementString("CountryID", _systemId);
            writer.WriteEndElement();
        }
    }
}
