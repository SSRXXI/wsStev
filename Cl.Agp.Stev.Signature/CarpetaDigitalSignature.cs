using System;
using System.Xml.Linq;
using Cl.Agp.Stev.Binding.Request;
using Cl.Agp.Stev.Binding.Response;
using Cl.Agp.Stev.Signature.Interfaces;
using Cl.Agp.Stev.Utils;
using System.Web;
using log4net;
using System.Linq;
using System.Text;
using System.IO;

namespace Cl.Agp.Stev.Signature
{
    public class CarpetaDigitalSignature : Template, ISoapCustomRequest<CargaDocumentoRequest>, ISoapCustomResponse<CargaDocumentoResponse>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GetEndpoint
        {
            get
            {
                return Util.GetValue("Endpoint:DocumentosPID");
            }

        }

        private string GetTemplate
        {
            get
            {
                return HttpContext.Current.Server.MapPath(Util.GetValue("template:CCD"));
            }
        }

        private string GetSoapAction
        {
            get
            {
                return Util.GetValue("SOAPAction:DocumentosPID");
            }

        }

        public CargaDocumentoResponse Deserialize(XDocument response)
        {
            CargaDocumentoResponse customResponse = new CargaDocumentoResponse();
            try
            {
                if (response.Descendants().Where(n => n.Name == "codigoresp").Count() != 0)
                {
                    customResponse.CodigoRespuesta = response.Descendants().Where(n => n.Name == "codigoresp").FirstOrDefault().Value.ToString();
                    customResponse.Glosa = response.Descendants().Where(n => n.Name == "glosa").FirstOrDefault().Value.ToString();
                    return customResponse;
                }
                else
                {
                    customResponse.Pantentes = response.Descendants().Where(n => n.Name == "PATENTE").FirstOrDefault().Value.ToString();
                    customResponse.Nro = response.Descendants().Where(n => n.Name == "NRO").FirstOrDefault().Value.ToString();
                    customResponse.FechaIngreso = DateTime.Parse(response.Descendants().Where(n => n.Name == "FECHA_ING").FirstOrDefault().Value.ToString());
                    customResponse.Nombre = response.Descendants().Where(n => n.Name == "NOMBRE").FirstOrDefault().Value.ToString();
                    customResponse.Output = response.Descendants().Where(n => n.Name == "OUTPUT").FirstOrDefault().Value.ToString();
                    return customResponse;
                }
            }
            catch (Exception e)
            {
                log.Error($"()=>, Exception : {e.Message} StackTrace : {e.StackTrace}");

                customResponse.CodigoRespuesta = "-103";
                customResponse.Glosa = "Se ha producido un error al invocar el servicio UploadFile, Error: " + e;
                return customResponse;
            }
            finally
            {
                if (Util.GetValue("SoapTrace").Equals("1"))
                {
                    Util.CapTrace("UploadFile-", response.ToString());
                }
            }
        }

        public XDocument GenerateQuery(CargaDocumentoRequest obj)
        {
            string plantillaBase = null;
            StringBuilder request = null;
            SoapClientCustom soap = new SoapClientCustom();
            try
            {
                TemplateDir = GetTemplate;
                plantillaBase = File.ReadAllText(TemplateDir);
                request = new StringBuilder(plantillaBase);
                request.Replace("param_consumidor", Util.IsNotNull(obj.Consumidor) ?? "");
                request.Replace("param_servicio", Util.IsNotNull(obj.Servicio) ?? "");
                request.Replace("param_file", Util.IsNotNull(obj.File) ?? "");
                request.Replace("param_patente", Util.IsNotNull(obj.Patente) ?? "");
                request.Replace("param_nro", Util.IsNotNull(obj.Nro) ?? "");
                request.Replace("param_tipo_sol", Util.IsNotNull(obj.TipoSolicitud) ?? "");
                request.Replace("param_tipo_doc", Util.IsNotNull(obj.TipoDocumento) ?? "");
                request.Replace("param_clasificacion", Util.IsNotNull(obj.Clasificacion) ?? "");
                request.Replace("param_fecha_ing", Util.IsNotNull(obj.FechaIngreso.ToString()) ?? "");
                request.Replace("param_nombre", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Nombre)) ?? "");

                XDocument resp = soap.PostSoapRequest(request.ToString(), GetEndpoint, GetSoapAction);
                return resp;
            }
            catch (Exception e)
            {
                log.Error($"()=>, Exception : {e.Message} StackTrace : {e.StackTrace}");
                return null;
            }
            finally
            {
                if (Util.GetValue("SoapTrace").Equals("1"))
                {
                    Util.CapTrace("UploadFile-", request.ToString());
                }
            }
        }
    }
}
