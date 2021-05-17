﻿using Cl.Agp.Stev.Binding.Request;
using Cl.Agp.Stev.Binding.Response;
using Cl.Agp.Stev.Signature.Interfaces;
using Cl.Agp.Stev.Utils;
using log4net;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Cl.Agp.Stev.Signature
{
    public class CertificadoTransferenciaSignature : Template, ISoapCustomRequest<ConsultaCertificadoRequest>, ISoapCustomResponse<ConsultaCertificadoResponse>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GetEndpoint
        {
            get
            {
                return Util.GetValue("Endpoint:EstadoSolicitud");
            }

        }
        private string GetTemplate
        {
            get
            {
                return HttpContext.Current.Server.MapPath(Util.GetValue("template:CSCERT"));
            }
        }
        private string GetSoapAction
        {
            get
            {
                return Util.GetValue("SOAPAction:CertificadoTransferencia");
            }
        }

        public XDocument GenerateQuery(ConsultaCertificadoRequest obj)
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
                request.Replace("param_tramite", "CONSULTA TRANSFERENCIA");
                request.Replace("param_servicio", "CONSULTA TRANSFERENCIA");
                request.Replace("param_nro_solicitud", Util.IsNotNull(obj.NroSolicitud.ToString()) ?? "");
                request.Replace("param_anio", Util.IsNotNull(obj.AnioSolicitud.ToString()) ?? "");
                request.Replace("param_ppu", Util.IsNotNull(obj.Ppu) ?? "");

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
                    Util.CapTrace("CertificadoTransferencia-", request.ToString());
                }
            }
        }

        public ConsultaCertificadoResponse Deserialize(XDocument response)
        {
            ConsultaCertificadoResponse customResponse = new ConsultaCertificadoResponse();
            try
            {
                customResponse.CodigoRespuesta = response.Descendants().Where(n => n.Name == "codigoresp").FirstOrDefault().Value.ToString() ?? "";
                customResponse.Glosa = response.Descendants().Where(n => n.Name == "glosa").FirstOrDefault().Value.ToString() ?? "";
                customResponse.Documento = response.Descendants().Where(n => n.Name == "documento").FirstOrDefault().Value.ToString() ?? "";

                return customResponse;
            }
            catch (Exception e)
            {
                customResponse.CodigoRespuesta = "-105";
                customResponse.Glosa = "Se ha producido un error al invocar el servicio CertificadoTransferencia, Error: " + e;
                log.Error($"()=>, Exception : {e.Message} StackTrace : {e.StackTrace}");
                return customResponse;
            }
            finally
            {
                if (Util.GetValue("SoapTrace").Equals("1"))
                {
                    Util.CapTrace("CertificadoTransferencia-", response.ToString());
                }
            }
        }
    }
}
