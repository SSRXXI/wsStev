using System;
using System.Xml.Linq;
using Cl.Agp.Stev.Binding.Request;
using Cl.Agp.Stev.Binding.Response;
using Cl.Agp.Stev.Signature.Interfaces;
using System.Web;
using Cl.Agp.Stev.Utils;
using log4net;
using System.Xml.XPath;
using Cl.Agp.Stev.Binding;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Cl.Agp.Stev.Signature
{
    public class LimitStevSignature : Template, ISoapCustomRequest<CreateLimitStevRequest>, ISoapCustomResponse<CreateLimitStevResponse>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GetEndpoint
        {
            get
            {
                return Util.GetValue("Endpoint:stev");
            }
        }
        private string GetTemplate
        {
            get
            {
                return HttpContext.Current.Server.MapPath(Util.GetValue("template:CRLIMSVT"));
            }
        }
        private string GetTemplateComunidad
        {
            get
            {
                return HttpContext.Current.Server.MapPath(Util.GetValue("template:nodo:comunidad"));
            }
        }
        private string GetTemplateReIngreso
        {
            get
            {
                return HttpContext.Current.Server.MapPath(Util.GetValue("template:nodo:re-ingreso"));
            }
        }

        private string GetSoapAction
        {
            get
            {
                return Util.GetValue("SOAPAction:createLimitStev");
            }
        }
        public CreateLimitStevResponse Deserialize(XDocument response)
        {
            CreateLimitStevResponse customResponse = new CreateLimitStevResponse();
            try
            {
                customResponse.CodigoRespuesta = response.Descendants().Where(n => n.Name == "codigoresp").FirstOrDefault().Value.ToString() ?? "";
                customResponse.Glosa = response.Descendants().Where(n => n.Name == "glosa").FirstOrDefault().Value.ToString() ?? "";
                if (response.XPathSelectElement("//solicitud") != null)
                {
                    customResponse.Solicitud = new Solicitud();
                    customResponse.Solicitud.Fecha = response.XPathSelectElement("//solicitud").Element("fecha").Value.ToString() ?? "";
                    customResponse.Solicitud.Hora = response.XPathSelectElement("//solicitud").Element("hora").Value.ToString() ?? "";
                    customResponse.Solicitud.NroSolicitud = int.Parse(response.XPathSelectElement("//solicitud").Element("numeroSol").Value.ToString() ?? "0");
                    customResponse.Solicitud.Oficina = response.XPathSelectElement("//solicitud").Element("oficina").Value.ToString() ?? "";
                    customResponse.Solicitud.Ppu = response.XPathSelectElement("//solicitud").Element("ppu").Value.ToString() ?? "";
                    customResponse.Solicitud.TipoSolicitud = response.XPathSelectElement("//solicitud").Element("tipoSol").Value.ToString() ?? "";
                }
                if (response.XPathSelectElement("//Observaciones") != null)
                {
                    customResponse.Observaciones = new List<Observacion>();

                    IEnumerable<XElement> observacionesXML = response.XPathSelectElements("//Observaciones");

                    foreach (XElement observacionXML in observacionesXML)
                    {
                        string detalle = observacionXML.Element("Observa").Value.ToString();
                        if (!string.IsNullOrEmpty(detalle))
                        {
                            customResponse.Observaciones.Add(
                                 new Observacion()
                                 {
                                     Detalle = observacionXML.Element("Observa").Value.ToString()
                                 }
                            );
                        }
                    }
                }

                return customResponse;
            }
            catch (Exception e)
            {
                customResponse.CodigoRespuesta = "-104";
                customResponse.Glosa = "Se ha producido un error al invocar el servicio LimiStev, Error: " + e;
                log.Error($"()=>, Exception : {e.Message} StackTrace : {e.StackTrace}");
                return customResponse;
            }
            finally
            {
                if (Util.GetValue("SoapTrace").Equals("1"))
                {
                    Util.CapTrace("LimitStev-", response.ToString());
                }
            }
        }

        public XDocument GenerateQuery(CreateLimitStevRequest obj)
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
                request.Replace("param_tramite", Util.IsNotNull(obj.Tramite) ?? "");
                /*Propietario Titular*/
                request.Replace("param_calidad", Util.IsNotNull(obj.Propietario.Titular.Calidad) ?? "");
                request.Replace("param_rut", Util.IsNotNull(obj.Propietario.Titular.Rut) ?? "");
                request.Replace("param_nombre", Util.IsNotNull(obj.Propietario.Titular.Nombre) ?? "");
                request.Replace("param_apellido_paterno", Util.IsNotNull(obj.Propietario.Titular.ApellidoPaterno) ?? "");
                request.Replace("param_apellido_materno", Util.IsNotNull(obj.Propietario.Titular.ApellidoMaterno) ?? "");
                request.Replace("param_email", Util.IsNotNull(obj.Propietario.Titular.Email) ?? "");
                /*Propietario Comunidad*/
                var nodoComunidadPropietario = new StringBuilder(File.ReadAllText(GetTemplateComunidad));
                nodoComunidadPropietario.Replace("param_es_comunidad", obj.Propietario.Comunidad.EsComunidad ? "S" : "N");
                nodoComunidadPropietario.Replace("param_cantidad", obj.Propietario.Comunidad.CantidadIntegrantes.ToString() ?? "0");
                request.Replace("{{nodo-comunidad-propietario}}", nodoComunidadPropietario.ToString());
                /*Acreedor*/
                request.Replace("param_acreedor_rut", Util.IsNotNull(obj.Acreedor.Rut) ?? "");
                request.Replace("param_acreedor_nombre", Util.IsNotNull(obj.Acreedor.Nombre) ?? "");
                /*Documento*/
                request.Replace("param_documento_tipo_documento", Util.IsNotNull(obj.Documento.TipoDocumento) ?? "");
                request.Replace("param_documento_numero", Util.IsNotNull(obj.Documento.Numero) ?? "");
                request.Replace("param_documento_lugar", Util.IsNotNull(obj.Documento.Lugar) ?? "");
                //request.Replace("param_documento_fecha", Util.IsNotNull(obj.Documento.Fecha.ToString()) ?? "");
                request.Replace("param_documento_fecha", "20210514");
                request.Replace("param_documento_autorizante", Util.IsNotNull(obj.Documento.Autorizante) ?? "");
                request.Replace("param_documento_observacion", Util.IsNotNull(obj.Documento.Observacion) ?? "");
                /*Operador*/
                request.Replace("param_operador_region", Util.IsNotNull(obj.Operador.RegionSolicitud) ?? "");
                request.Replace("param_operador_usuario", Util.IsNotNull(obj.Operador.RunUsuarioSolicitante) ?? "");
                request.Replace("param_operador_empresa", Util.IsNotNull(obj.Operador.RutEmpresaSolicitante) ?? "");
                /*Solicitante*/
                request.Replace("param_solicitante_calidad", Util.IsNotNull(obj.Solicitante.Persona.Calidad) ?? "");
                request.Replace("param_solicitante_rut", Util.IsNotNull(obj.Solicitante.Persona.Rut) ?? "");
                request.Replace("param_solicitante_nombre", Util.IsNotNull(obj.Solicitante.Persona.Nombre) ?? "");
                request.Replace("param_solicitante_apellido_paterno", Util.IsNotNull(obj.Solicitante.Persona.ApellidoPaterno) ?? "");
                request.Replace("param_solicitante_apellido_materno", Util.IsNotNull(obj.Solicitante.Persona.ApellidoMaterno) ?? "");
                request.Replace("param_solicitante_email", Util.IsNotNull(obj.Solicitante.Persona.Email) ?? "");
                request.Replace("param_solicitante_calle", Util.IsNotNull(obj.Solicitante.Direccion.Calle) ?? "");
                request.Replace("param_solicitante_direccion", Util.IsNotNull(obj.Solicitante.Direccion.LetraDireccion) ?? "");
                request.Replace("param_solicitante_nro_direccion", Util.IsNotNull(obj.Solicitante.Direccion.Numero.ToString()) ?? "");
                request.Replace("param_solicitante_resto_direccion", Util.IsNotNull(obj.Solicitante.Direccion.RestoDomicilio) ?? "");
                request.Replace("param_solicitante_telefono", Util.IsNotNull(obj.Solicitante.Direccion.Telefono.ToString()) ?? "");
                request.Replace("param_solicitante_comuna", Util.IsNotNull(obj.Solicitante.Direccion.Comuna) ?? "");
                request.Replace("param_solicitante_codigo_postal", Util.IsNotNull(obj.Solicitante.Direccion.CodigoPostal) ?? "");
                /*Vehiculo*/
                request.Replace("param_vehiculo_ppu", Util.IsNotNull(obj.Vehiculo.Ppu) ?? "");
                request.Replace("param_vehiculo_nro_motor", Util.IsNotNull(obj.Vehiculo.NroMotor) ?? "");
                request.Replace("param_vehiculo_nro_chasis", Util.IsNotNull(obj.Vehiculo.Chasis) ?? "");
                request.Replace("param_vehiculo_nro_serie", Util.IsNotNull(obj.Vehiculo.NroSerie) ?? "");
                request.Replace("param_vehiculo_nro_vin", Util.IsNotNull(obj.Vehiculo.NroVin) ?? "");

                if (obj.Reingreso != null)
                {
                    var nodoReIngreso = new StringBuilder(File.ReadAllText(GetTemplateReIngreso));
                    nodoReIngreso.Replace("param_reingreso_ppu", Util.IsNotNull(obj.Reingreso.PPU) ?? "");
                    nodoReIngreso.Replace("param_reingreso_nro_solicitud", Util.IsNotNull(obj.Reingreso.NroSolicitud.ToString()) ?? "");
                    nodoReIngreso.Replace("param_reingreso_fecha_solicitud", Util.IsNotNull(obj.Reingreso.FechaSolicitud.ToString()) ?? "");
                    nodoReIngreso.Replace("param_reingreso_nro_res_exenta", Util.IsNotNull(obj.Reingreso.NroResolucionExenta.ToString()) ?? "");
                    nodoReIngreso.Replace("param_reingreso_fecha_res_exenta", Util.IsNotNull(obj.Reingreso.FechaResolucionExenta.ToString()) ?? "");
                    request.Replace("{{nodo-re-ingreso}}", nodoReIngreso.ToString());
                }
                else
                {
                    request.Replace("{{nodo-re-ingreso}}", "");
                }

                XDocument resp = soap.PostSoapRequest(request.ToString(), GetEndpoint, GetSoapAction);
                return resp;
            }
            catch (Exception e)
            {
                log.Error($"()=>, Exception : {e.Message} StackTrace : {e.StackTrace}");
                return null;
            }
        }
    }
}
