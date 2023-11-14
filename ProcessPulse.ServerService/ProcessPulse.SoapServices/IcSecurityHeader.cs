using System.ServiceModel.Channels;
using System.Xml;

namespace ProcessPulse.ServerService.ProcessPulse.SoapServices
{ 
  internal class IcSecurityHeader : MessageHeader
  {
    public override string Name => "wsse:Security";

    public override string Namespace => null;

    protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
    {
      writer.WriteXmlnsAttribute("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
      writer.WriteXmlnsAttribute("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

      writer.WriteStartElement("wsse", "UsernameToken", null);
      writer.WriteAttributeString("wsu:Id", "UsernameToken-284646D189449068C51562588680002142");

      writer.WriteStartElement("wsse", "Username", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
      
//#if (DEBUG || Test)
//      writer.WriteValue("fleet");
//#else
//      writer.WriteValue("fleet");
//#endif
      writer.WriteEndElement();

      writer.WriteStartElement("wsse", "Password", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
      writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
     
//#if (DEBUG || Test)
//      writer.WriteValue("fleet#abc123");
//#else
//      writer.WriteValue("fleet#gta132");
//#endif
      writer.WriteEndElement();

      writer.WriteEndElement();
    }
  }
}