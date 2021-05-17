using Cl.Agp.Stev.Binding.Request;
using Cl.Agp.Stev.Binding.Response;
using Cl.Agp.Stev.Signature;
using Cl.Agp.Stev.Utils;
using log4net;
using System;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace WsAgpStev
{
    /// <summary>
    /// Summary description for StevAgp
    /// </summary>
    [WebService(Namespace = "http://agpsa-stev.cl/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class StevAgp : System.Web.Services.WebService
    {
        private static ILog _Log;

        public StevAgp()
        {
            _Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        [WebMethod]
        public Etapa1Response CreateStevEtapa1(Etapa1StevRequest request)
        {
            Etapa1Response response = new Etapa1Response();
            response.CodigoRespuesta = "Ok";
            response.Glosa = "Etapa 1 realizada correctamente";
            try
            {
                ConsultarAnotacionesVigentesStev(request.consultaAnotacionesVigentesRequest);
                CreateStev(request.createStevRequest);
                CargarDocumentoCarpetaDigital(request.cargaDocumentoRequest);
                CreateLimitStev(request.createLimitStevRequest);
            }
            catch (Exception ex)
            {
                response.CodigoRespuesta = "Error";
                response.Glosa = ex.Message;

            }

            return response;

        }

        [WebMethod]
        public Etapa2Response CreateStevEtapa2(Etapa2StevRequest request)
        {
            Etapa2Response response = new Etapa2Response();
            response.CodigoRespuesta = "Ok";
            response.Glosa = "Etapa 2 realizada correctamente";
            try
            {
                ObtenerCertificadoStev(request.consultaCertificadoRequest);
                ComprobarEstadoSolicitud(request.estadoSolicitudRequest);
                ConsultaLimitacionL(request.consultaLimitacionRequest);
            }
            catch (Exception ex)
            {
                response.CodigoRespuesta = "Error";
                response.Glosa = ex.Message;

            }
                
            return response;

        }

        [WebMethod]
        public CreateStevResponse CreateStev(CreateStevRequest request)
        {
           if (request.Documento.TipoDocumento == "CONTRATO PRIVADO ELECTRONICO" || request.Documento.TipoDocumento == "ESCRITURA PUB ELEC")
            {
                request.Documento.RutEmisor = "";
            }
            GlobalContext.Properties["ip"] = Util.GetIpDirection(HttpContext.Current.Request);
            StevSignature stevSignature = new StevSignature();
            CreateStevResponse createStevResponse = new CreateStevResponse();
            XDocument response = stevSignature.GenerateQuery(request);
            string path = Server.MapPath("~/App_Data/Respuesta_PIDF_CreaStev.xml");
            //XDocument document = XDocument.Load(path);
            createStevResponse = stevSignature.Deserialize(response);
            return createStevResponse;
        }

        [WebMethod]
        public CreateLimitStevResponse CreateLimitStev(CreateLimitStevRequest request)
        {
            request.Propietario.Titular.Email = "spievcertificacion@agpsa.cl";
            GlobalContext.Properties["ip"] = Util.GetIpDirection(HttpContext.Current.Request);
            LimitStevSignature lsSignature = new LimitStevSignature();
            CreateLimitStevResponse createLimitStevResponse = new CreateLimitStevResponse();
            XDocument response = lsSignature.GenerateQuery(request);
            //string path = Server.MapPath("~/App_Data/RESPUESTA_LimTransf.xml");
            //XDocument document = XDocument.Load(path);
            createLimitStevResponse = lsSignature.Deserialize(response);
            return createLimitStevResponse;
        }

        [WebMethod]
        public ConsultaAnotacionesVigentesResponse ConsultarAnotacionesVigentesStev(ConsultaAnotacionesVigentesRequest request)
        {
            GlobalContext.Properties["ip"] = Util.GetIpDirection(HttpContext.Current.Request);
            AnotacionesVigentesSignature avsignature = new AnotacionesVigentesSignature();
            XDocument response = avsignature.GenerateQuery(request);
            //string path = Server.MapPath("~/App_Data/Respuesta_CERAVGT_AGP.xml");
            //XDocument document = XDocument.Load(path);
            return avsignature.Deserialize(response);
        }

        [WebMethod]
        public ConsultaCertificadoResponse ObtenerCertificadoStev(ConsultaCertificadoRequest request)
        {
            GlobalContext.Properties["ip"] = Util.GetIpDirection(HttpContext.Current.Request);
            CertificadoTransferenciaSignature ctsignature = new CertificadoTransferenciaSignature();
            XDocument response = ctsignature.GenerateQuery(request);

            string path = Server.MapPath("~/App_Data/Respuesta_Transferencia.xml");
            XDocument document = XDocument.Load(path);
            return ctsignature.Deserialize(response);

            object respondido = ctsignature.Deserialize(document);
            if (respondido != null)
            {

            }
            else
            {

            }
            return ctsignature.Deserialize(document);
        }

        [WebMethod]
        public CargaDocumentoResponse CargarDocumentoCarpetaDigital(CargaDocumentoRequest request)
        {
            GlobalContext.Properties["ip"] = Util.GetIpDirection(HttpContext.Current.Request);
            CarpetaDigitalSignature ctsignature = new CarpetaDigitalSignature();
            XDocument response = ctsignature.GenerateQuery(request);
            return ctsignature.Deserialize(response);
        }
        [WebMethod]
        public EstadoSolicitudResponse ComprobarEstadoSolicitud(EstadoSolicitudRequest request)
        {
            GlobalContext.Properties["ip"] = Util.GetIpDirection(HttpContext.Current.Request);
            EstadoSolicitudSignature esolsignature = new EstadoSolicitudSignature();
            XDocument response = esolsignature.GenerateQuery(request);
            return esolsignature.Deserialize(response);
        }

        [WebMethod]
        public ConsultaLimitacionResponse ConsultaLimitacionL(ConsultaLimitacionRequest request)
        {
            ConsultaLimitacionResponse resp = new ConsultaLimitacionResponse();
            resp.CodigoRespuesta = "222";
            //GlobalContext.Properties["ip"] = Util.GetIpDirection(HttpContext.Current.Request);
            //ConsultaLimitacionSignature cslimsignature = new ConsultaLimitacionSignature();
            //XDocument response = cslimsignature.GenerateQuery(request);
            //return cslimsignature.Deserialize(response);
            return resp;
        }

    }
}
