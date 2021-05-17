using log4net;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Cl.Agp.Stev.Signature
{
    internal sealed class SignedXmlWithId : SignedXml
    {
        public SignedXmlWithId(XmlDocument xml)
            : base(xml)
        {
        }

        public SignedXmlWithId(XmlElement xmlElement)
            : base(xmlElement)
        {

        }

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            // check to see if it's a standard ID reference
            XmlElement idElem = base.GetIdElement(doc, id);

            if (idElem == null)
            {
                XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

                idElem = doc.SelectSingleNode("//*[@wsu:Id=\"" + id + "\"]", nsManager) as XmlElement;
            }

            return idElem;
        }

    }

    #region 
    //Classes Auxiliares
    public class WS_4117909A
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private HttpStatusCode cLastCode;
        private X509Certificate2 cCertificado;
        private string cFileName;
        private string Url;

        const string STR_SOAPENV = "http://schemas.xmlsoap.org/soap/envelope/";
        const string STR_WSSE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        const string STR_WSU = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        const string STR_DS = "http://www.w3.org/2000/09/xmldsig#";
        const string STR_EC = "http://www.w3.org/2001/10/xml-exc-c14n#";

        const string sSoapEnvelope =
              @"<?xml version=""1.0"" encoding=""utf-8""?>
                <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"">
                  <SOAP-ENV:Header>
                    <wsse:Security 
                       xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" 
                       xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""
                       SOAP-ENV:mustUnderstand=""1"">
                       <wsse:BinarySecurityToken EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"" 
                                                 ValueType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3"" 
                                                 wsu:Id=""X509-EC95425802FB9F663F15021186132611""> 
                       </wsse:BinarySecurityToken>
                    </wsse:Security> 
                  </SOAP-ENV:Header>
                  <SOAP-ENV:Body xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"" wsu:Id=""id-1"">
                  </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";

        public X509Certificate2 Certificado
        {
            get { return cCertificado; }
            set { cCertificado = value; }
        }

        public string FileName
        {
            get { return cFileName; }
            set { cFileName = value; }
        }

        public HttpStatusCode lastCode { get { return cLastCode; } }

        public WS_4117909A(
            X509Certificate2 pCertificado,
            string pUrl)
        {
            Certificado = pCertificado;
            Url = pUrl;
        }

        public bool ValidateSoapBodySignature(XmlDocument doc, X509Certificate2 cert)
        {

            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("SOAP-ENV", STR_SOAPENV);
            ns.AddNamespace("wsse", STR_WSSE);
            ns.AddNamespace("wsu", STR_WSU);
            ns.AddNamespace("ds", STR_WSU);
            ns.AddNamespace("ec", STR_WSU);

            bool passes = true;
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = "XML_DSIG_RSA_KEY";
            RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);
            SignedXml signedXml = new SignedXmlWithId(doc);
            XmlNodeList nodeList = doc.GetElementsByTagName("Signature");
            if (nodeList.Count == 0) nodeList = doc.GetElementsByTagName("ds:Signature");
            XmlNodeList certificates = doc.GetElementsByTagName("wsse:BinarySecurityToken");
            X509Certificate2 dcert2 = new X509Certificate2(Convert.FromBase64String(certificates[0].InnerText));
            foreach (XmlElement element in nodeList)
            {
                signedXml.LoadXml(element);
                passes = passes && signedXml.CheckSignature(dcert2, true);
            }

            return passes;
        }

        /// <summary>
        /// Create a soap webrequest to [Url]
        /// </summary>
        /// <returns></returns>
        public static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        public XmlDocument AssinaSOAP(string value)
        {

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(sSoapEnvelope);

            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("SOAP-ENV", STR_SOAPENV);
            ns.AddNamespace("wsse", STR_WSSE);
            ns.AddNamespace("wsu", STR_WSU);
            ns.AddNamespace("ds", STR_WSU);
            ns.AddNamespace("ec", STR_WSU);

            // *** Grab the body element - this is what we create the signature from
            XmlElement body = doc.DocumentElement.SelectSingleNode(@"//SOAP-ENV:Body", ns) as XmlElement;
            if (body == null)
                throw new ApplicationException("No body tag found");

            // *** Fill the body
            body.InnerXml = value;

            // *** Signed XML will create Xml Signature - Xml fragment
            SignedXmlWithId signedXml = new SignedXmlWithId(doc);

            // *** Create a KeyInfo structure
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.Id = "KI-EC95425802FB9F663F15021186132692";

            KeyInfoNode keyInfoNode = new KeyInfoNode();
            XmlElement wsseSec = doc.CreateElement("wsse", "SecurityTokenReference", STR_WSSE);
            wsseSec.SetAttribute("xmlns:wsu", STR_WSU);
            wsseSec.SetAttribute("Id", STR_WSU, "STR-EC95425802FB9F663F15021186132713");
            XmlElement wsseRef = doc.CreateElement("wsse", "Reference", STR_WSSE);
            wsseRef.SetAttribute("URI", "#X509-EC95425802FB9F663F15021186132611");
            wsseRef.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            wsseSec.AppendChild(wsseRef);
            keyInfoNode.Value = wsseSec;

            keyInfo.AddClause(keyInfoNode);

            // *** The actual key for signing - MAKE SURE THIS ISN'T NULL!
            signedXml.SigningKey = this.Certificado.PrivateKey;

            // *** provide the certficate info that gets embedded - note this is only
            // *** for specific formatting of the message to provide the cert info
            signedXml.KeyInfo = keyInfo;

            // *** Again unusual - meant to make the document match template
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            // Set the InclusiveNamespacesPrefixList property.        
            XmlDsigExcC14NTransform canMethod = (XmlDsigExcC14NTransform)signedXml.SignedInfo.CanonicalizationMethodObject;
            canMethod.InclusiveNamespacesPrefixList = "SOAP-ENV";

            // *** Now create reference to sign: Point at the Body element
            Reference reference = new Reference();
            reference.Uri = "#id-1";  // reference wsu:Id=id-6 section in same doc

            // Add an enveloped transformation to the reference.
            XmlDsigExcC14NTransform env = new XmlDsigExcC14NTransform();
            env.InclusiveNamespacesPrefixList = "";
            reference.AddTransform(env); // required to match doc


            signedXml.AddReference(reference);

            // *** Finally create the signature
            signedXml.ComputeSignature();

            // *** wsse:Security element
            XmlElement soapSignature = doc.DocumentElement.SelectSingleNode(@"//wsse:Security", ns) as XmlElement;
            if (soapSignature == null)
                throw new ApplicationException("No wsse:Security tag found");

            // *** Create wsse:BinarySecurityToken element
            XmlElement soapToken = doc.DocumentElement.SelectSingleNode(@"//wsse:BinarySecurityToken", ns) as XmlElement;
            if (soapToken == null)
                throw new ApplicationException("No wsse:BinarySecurityToken tag found");

            var export = this.Certificado.Export(X509ContentType.Cert);
            var base64 = Convert.ToBase64String(export);
            soapToken.InnerText = base64;

            // *** Result is an XML node with the signature detail below it
            // *** Now let's add the sucker into the SOAP-HEADER
            XmlElement signedElement = signedXml.GetXml();
            XmlAttribute sId = doc.CreateAttribute("Id");
            sId.Value = "SIG-2";
            signedElement.Attributes.Append(sId);

            // *** And add our signature as content
            soapSignature.AppendChild(signedElement);

            // *** Now add the signature header into the master header
            XmlElement soapHeader = doc.DocumentElement.SelectSingleNode("//SOAP-ENV:Header", ns) as XmlElement;
            if (soapHeader == null)
                throw new ApplicationException("No SOAP-ENV:Header tag found");

            // *** Validate
            // var pass = ValidateSoapBodySignature(doc, this.Certificado);

            return doc;
        }

        public String ConsultarNfseRps(String value)
        {
            HttpWebRequest request = CreateWebRequest(this.Url, "");
            XmlDocument soapEnvelopeXml = AssinaSOAP(value);
            soapEnvelopeXml.RemoveChild(soapEnvelopeXml.FirstChild);
            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
                soapEnvelopeXml.Save(FileName.Replace(".xml", ".soap.xml")); //salvar arquivo retorno
            }

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();
                        return soapResult;
                    }
                }

            }
            catch (WebException wex)
            {
                cLastCode = (wex.Response as HttpWebResponse).StatusCode;
                return new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
            }

        }

        public string AgregarCertificado(string requestBase)
        {
            try
            {
                XmlDocument soapEnvelopeXml = AssinaSOAP(requestBase);
                soapEnvelopeXml.RemoveChild(soapEnvelopeXml.FirstChild);
                return soapEnvelopeXml.InnerXml;
            }
            catch (Exception e)
            {
                log.Error($"()=>, Exception : {e.Message}, Codigo Error : {8003}");
                return null;
            }
        }

        public String ConsultarNfsePorRps(String value)
        {
            return this.ConsultarNfseRps(value);
        }

        public String EnviarLoteRps(String value)
        {
            return null;
        }

    }
    #endregion
}
