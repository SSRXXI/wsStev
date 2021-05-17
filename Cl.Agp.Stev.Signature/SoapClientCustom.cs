using Cl.Agp.Stev.Utils;
using log4net;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Cl.Agp.Stev.Signature
{
    public class SoapClientCustom
    {
        #region Campos y Constantes 
        private static readonly ILog _Log;
        private static readonly HttpClient _HttpClient;
        #endregion
        #region Constructor

        static SoapClientCustom()
        {
            _Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _HttpClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip, ClientCertificateOptions = ClientCertificateOption.Manual });
            _HttpClient.Timeout = TimeSpan.FromTicks(Convert.ToInt64(Util.GetValue("segtimeout")));
        }
        
        #endregion
        #region Propiedades
        private string X509CertificadoRuta
        {
            get
            {
                return HttpContext.Current.Server.MapPath(Util.GetValue("x509certificadoruta"));
            }
        }

        private string X509CertificadoPassword
        {
            get
            {
                return Util.GetValue("x509certificadopassword");
            }
        }

        #endregion
        private string LoadSignature(string _requestTemplate, string _urlEnpoint)
        {
            string requestFirma = string.Empty;
            try
            {
                X509Certificate2 cert = new X509Certificate2(X509CertificadoRuta, X509CertificadoPassword, X509KeyStorageFlags.MachineKeySet);
                WS_4117909A ws1 = new WS_4117909A(cert, _urlEnpoint);
                var doc = XDocument.Parse(_requestTemplate);
                var request = doc.ToString(SaveOptions.DisableFormatting);
                string firmado = ws1.AgregarCertificado(request);
                return firmado;
            }
            catch (Exception e)
            {
                return requestFirma;
            }
        }

        public XDocument PostSoapRequest(string soapRequest, string _urlEndpoint, string _soapAction)
        {
            try
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(_urlEndpoint),
                    Method = HttpMethod.Post
                };

                request.Content = new StringContent(LoadSignature(soapRequest, _urlEndpoint), Encoding.UTF8, "text/xml");
                request.Headers.Clear();
                _HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                request.Headers.Add("SOAPAction", _soapAction);

                if (!bool.Parse(Util.GetValue("Soap:valida-ssl")))
                {
                    ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                }

                var x = request.ToString();

                HttpResponseMessage response = _HttpClient.SendAsync(request).Result;
                Task<Stream> streamTask = response.Content.ReadAsStreamAsync();
                Stream stream = streamTask.Result;
                var sr = new StreamReader(stream);
                XDocument soapResponse = XDocument.Load(sr);
                    return soapResponse;
            }
            catch (AggregateException e)
            {
                if (e.InnerException is TaskCanceledException)
                {
                    _Log.Error($"()=>, Exception : {e.Message} StackTrace: {e.StackTrace}, Codigo Error : {8013}");
                    throw e.InnerException;
                }
                if (e.InnerException is HttpRequestException)
                {
                    foreach (HttpRequestException exeption in e.InnerExceptions)
                    {
                        _Log.Error($"()=>, Exception : {exeption.Message}, InnerExeption : {exeption.InnerException?.InnerException}, Codigo Error : {8014}");
                    }
                }
                else
                {
                    _Log.Error($"()=>, Exception : {e.Message}, InnerExeption : {e?.InnerException } InnerExeption : {e?.InnerException?.InnerException}, Codigo Error : {8014}");
                }
                return null;
            }
            catch (Exception e)
            {
                _Log.Error($"()=>, Exception : {e.Message}, InnerException : {e?.InnerException}, Codigo Error : {8015}");
                return null;
            }
        }
    }
}
