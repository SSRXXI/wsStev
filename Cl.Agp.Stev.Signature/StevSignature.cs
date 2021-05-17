using Cl.Agp.Stev.Binding;
using Cl.Agp.Stev.Binding.Request;
using Cl.Agp.Stev.Binding.Response;
using Cl.Agp.Stev.Signature.Interfaces;
using Cl.Agp.Stev.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Cl.Agp.Stev.Signature
{
    public class StevSignature : Template, ISoapCustomRequest<CreateStevRequest>, ISoapCustomResponse<CreateStevResponse>
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
                return HttpContext.Current.Server.MapPath(Util.GetValue("template:CRSTV"));
            }
        }
        private string GetTemplateReIngreso
        {
            get
            {
                return HttpContext.Current.Server.MapPath(Util.GetValue("template:nodo:re-ingreso"));
            }
        }
        private string GetTemplateComprador
        {
            get
            {
                return HttpContext.Current.Server.MapPath(Util.GetValue("template:nodo:comprador"));
            }
        }

        private string GetSoapAction
        {
            get
            {
                return Util.GetValue("SOAPAction:createStev");
            }
        }
        public CreateStevResponse Deserialize(XDocument response)
        {
            CreateStevResponse customResponse = new CreateStevResponse();
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
                    if (customResponse.Observaciones.Count > 0)
                    {
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
                    else
                    {
                        customResponse.Observaciones.Add(
                                     new Observacion()
                                     {
                                         Detalle = "Sin Observaciones"
                                     }
                                );
                    }

                }

                return customResponse;
            }
            catch (Exception e)
            {
                customResponse.CodigoRespuesta = "-104";
                customResponse.Glosa = "Se ha producido un error al invocar el servicio CreateStev, Error: " + e;
                log.Error($"()=>, Exception : {e.Message} StackTrace : {e.StackTrace}");
                return customResponse;
            }
            finally
            {
                if (Util.GetValue("SoapTrace").Equals("1"))
                {
                    Util.CapTrace("CreateStev-", response.ToString());
                }
            }
        }

        public XDocument GenerateQuery(CreateStevRequest obj)
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
                request.Replace("param_tramite", Util.IsNotNull(obj.Tramite) ?? "");
                /*Vehiculo*/
                request.Replace("param_vehiculo_ppu", Util.IsNotNull(obj.Vehiculo.Ppu) ?? "");
                request.Replace("param_vehiculo_dv", Util.IsNotNull(obj.Vehiculo.Dv) ?? "");
                request.Replace("param_vehiculo_tipo_vehiculo", Util.IsNotNull(obj.Vehiculo.TipoVehiculo.Trim()) ?? "");
                request.Replace("param_vehiculo_marca", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Vehiculo.Marca.Trim())) ?? "");
                request.Replace("param_vehiculo_modelo", Util.IsNotNull(obj.Vehiculo.Modelo.Trim()) ?? "");
                request.Replace("param_vehiculo_anio_fabricacion", Util.IsNotNull(obj.Vehiculo.AnioFabricacion.ToString()) ?? "");
                request.Replace("param_vehiculo_color", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Vehiculo.Color)) ?? "");
                request.Replace("param_vehiculo_nro_motor", Util.IsNotNull(obj.Vehiculo.NroMotor) ?? "");
                request.Replace("param_vehiculo_chasis", Util.IsNotNull(obj.Vehiculo.Chasis) ?? "");
                request.Replace("param_vehiculo_nro_serie", Util.IsNotNull(obj.Vehiculo.NroSerie) ?? "");
                request.Replace("param_vehiculo_nro_vin", Util.IsNotNull(obj.Vehiculo.NroVin) ?? "");
                /*Vendedor*/
                request.Replace("param_vendedor_es_comunidad", Util.IsNotNull(obj.Vendedor.Comunidad.EsComunidad ? "SI" : "NO") ?? "NO");
                request.Replace("param_vendedor_cantidad", Util.IsNotNull(obj.Vendedor.Comunidad.CantidadIntegrantes.ToString()) ?? "0");
                request.Replace("param_vendedor_calidad", Util.IsNotNull(obj.Vendedor.Persona.Calidad) ?? "");
                request.Replace("param_vendedor_rut", Util.IsNotNull(obj.Vendedor.Persona.Rut) ?? "");
                request.Replace("param_vendedor_nombre", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Vendedor.Persona.Nombre)) ?? "");
                request.Replace("param_vendedor_apellido_paterno", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Vendedor.Persona.ApellidoPaterno)) ?? "");
                request.Replace("param_vendedor_apellido_materno", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Vendedor.Persona.ApellidoMaterno)) ?? "");
                request.Replace("param_vendedor_email", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Vendedor.Persona.Email)) ?? "");
                /*Comprador*/
                request.Replace("param_comprador_es_comunidad", Util.IsNotNull(obj.Comprador.Comunidad.EsComunidad ? "SI" : "NO") ?? "NO");
                //request.Replace("param_comprador_es_comunidad", "false");
                request.Replace("param_comprador_cantidad", Util.IsNotNull(obj.Comprador.Comunidad.CantidadIntegrantes.ToString()) ?? "0");
                //request.Replace("param_comprador_cantidad", "0");

                var listadoCompradores = new StringBuilder();
                foreach (Compran compradorSingle in obj.Comprador.ListadoCompradores)
                {
                    var nodoComprador = new StringBuilder(File.ReadAllText(GetTemplateComprador));
                    nodoComprador.Replace("param_comprador_calidad", Util.IsNotNull(compradorSingle.Persona.Calidad) ?? "");
                    nodoComprador.Replace("param_comprador_rut", Util.IsNotNull(compradorSingle.Persona.Rut) ?? "");
                    nodoComprador.Replace("param_comprador_nombre", Util.IsNotNull(Util.ConvertSpecialCharacters(compradorSingle.Persona.Nombre)) ?? "");
                    nodoComprador.Replace("param_comprador_apellido_paterno", Util.IsNotNull(Util.ConvertSpecialCharacters(compradorSingle.Persona.ApellidoPaterno)) ?? "");
                    nodoComprador.Replace("param_comprador_apellido_materno", Util.IsNotNull(Util.ConvertSpecialCharacters(compradorSingle.Persona.ApellidoMaterno)) ?? "");
                    nodoComprador.Replace("param_comprador_email", Util.IsNotNull(Util.ConvertSpecialCharacters(compradorSingle.Persona.Email)) ?? "");
                    nodoComprador.Replace("param_comprador_calle", Util.IsNotNull(Util.ConvertSpecialCharacters(compradorSingle.Direccion.Calle)) ?? "");
                    nodoComprador.Replace("param_comprador_direccion", Util.IsNotNull(Util.ConvertSpecialCharacters(compradorSingle.Direccion.LetraDireccion)) ?? "");
                    nodoComprador.Replace("param_comprador_nro_direccion", Util.IsNotNull(compradorSingle.Direccion.Numero.ToString()) ?? "");
                    nodoComprador.Replace("param_comprador_resto_direccion", Util.IsNotNull(Util.ConvertSpecialCharacters(compradorSingle.Direccion.RestoDomicilio)) ?? "");
                    nodoComprador.Replace("param_comprador_telefono", Util.IsNotNull(compradorSingle.Direccion.Telefono.ToString()) ?? "");
                    nodoComprador.Replace("param_comprador_comuna", Util.IsNotNull(compradorSingle.Direccion.Comuna) ?? "");
                    nodoComprador.Replace("param_comprador_codigo_postal", Util.IsNotNull(Util.ConvertSpecialCharacters(compradorSingle.Direccion.CodigoPostal)) ?? "");
                    listadoCompradores.AppendLine(nodoComprador.ToString());
                }
                request.Replace("{{nodo-listado-compradores}}", listadoCompradores.ToString());

                /*Estipulante*/
                if (obj.Estipulante != null)
                {
                    request.Replace("param_estipulante_calidad", Util.IsNotNull(obj.Estipulante.Persona.Calidad) ?? "");
                    request.Replace("param_estipulante_rut", Util.IsNotNull(obj.Estipulante.Persona.Rut) ?? "");
                    request.Replace("param_estipulante_nombre", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Estipulante.Persona.Nombre)) ?? "");
                    request.Replace("param_estipulante_apellido_paterno", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Estipulante.Persona.ApellidoPaterno)) ?? "");
                    request.Replace("param_estipulante_apellido_materno", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Estipulante.Persona.ApellidoMaterno)) ?? "");
                    request.Replace("param_estipulante_email", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Estipulante.Persona.Email)) ?? "");
                    request.Replace("param_estipulante_calle", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Estipulante.Direccion.Calle)) ?? "");
                    request.Replace("param_estipulante_direccion", Util.IsNotNull(obj.Estipulante.Direccion.LetraDireccion) ?? "");
                    request.Replace("param_estipulante_nro_direccion", Util.IsNotNull(obj.Estipulante.Direccion.Numero.ToString()) ?? "");
                    request.Replace("param_estipulante_resto_direccion", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Estipulante.Direccion.RestoDomicilio)) ?? "");
                    request.Replace("param_estipulante_telefono", Util.IsNotNull(obj.Estipulante.Direccion.Telefono.ToString()) ?? "");
                    request.Replace("param_estipulante_comuna", Util.IsNotNull(obj.Estipulante.Direccion.Comuna) ?? "");
                    request.Replace("param_estipulante_codigo_postal", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Estipulante.Direccion.CodigoPostal)) ?? "");
                    request.Replace("param_estipulante_prohivicion", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Estipulante.Prohivicion)) ?? "");
                }
                else
                {
                    request.Replace("param_estipulante_calidad", "");
                    request.Replace("param_estipulante_calidad", "");
                    request.Replace("param_estipulante_rut", "");
                    request.Replace("param_estipulante_nombre", "");
                    request.Replace("param_estipulante_apellido_paterno", "");
                    request.Replace("param_estipulante_apellido_materno", "");
                    request.Replace("param_estipulante_email", "");
                    request.Replace("param_estipulante_calle", "");
                    request.Replace("param_estipulante_direccion", "");
                    request.Replace("param_estipulante_nro_direccion", "");
                    request.Replace("param_estipulante_resto_direccion", "");
                    request.Replace("param_estipulante_telefono", "");
                    request.Replace("param_estipulante_comuna", "");
                    request.Replace("param_estipulante_codigo_postal", "");
                    request.Replace("param_estipulante_prohivicion", "");
                }
                /*Documento*/
                request.Replace("param_documento_tipo_documento", Util.IsNotNull(obj.Documento.TipoDocumento) ?? "");
                if (obj.Documento.TipoDocumento == "CONTRATO PRIVADO ELECTRONICO")
                {
                    request.Replace("param_documento_naturaleza", "COMPRAVENTA");
                }
                else if (obj.Documento.TipoDocumento == "FACTURA ELECTRONICA")
                {
                    request.Replace("param_documento_naturaleza", "COMPRAVENTA");
                }
                else if (obj.Documento.TipoDocumento == "FACTURA ELECTRONICA EXENTA")
                {
                    request.Replace("param_documento_naturaleza", "COMPRAVENTA");
                }
                else if (obj.Documento.TipoDocumento == "ESCRITURA PUB ELECTRONICA")
                {
                    request.Replace("param_documento_naturaleza", "COMPRAVENTA");
                }

                request.Replace("param_documento_numero", Util.IsNotNull(obj.Documento.Numero) ?? "");
                request.Replace("param_documento_lugar", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Documento.Lugar)) ?? "");
                request.Replace("param_documento_fecha", Util.IsNotNull(obj.Documento.Fecha.ToString("yyyyMMdd")) ?? "");
                //request.Replace("param_documento_autorizante", Util.IsNotNull(obj.Documento.Autorizante) ?? "");
                //request.Replace("param_documento_observacion", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Documento.Observacion)) ?? "");
                //Util.IsNotNull(obj.Documento.RutEmisor)
                request.Replace("param_documento_moneda", "$");
                request.Replace("param_documento_kilometraje", Util.IsNotNull(obj.Documento.Kilometraje) ?? "");
                request.Replace("param_documento_rut_emisor", Util.IsNotNull(obj.Documento.RutEmisor) ?? "");
                request.Replace("param_documento_codigo_notaria", Util.IsNotNull(obj.Documento.CodigoNotaria) ?? "");
                request.Replace("param_documento_total", Util.IsNotNull(obj.Documento.Total.ToString()) ?? "");

                /*Impuesto*/
                request.Replace("param_impuesto_codigo_cid", Util.IsNotNull(obj.Impuesto.CodigoCID) ?? "");
                request.Replace("param_documento_monto_pagado", Util.IsNotNull(obj.Impuesto.MontoPagado.ToString()) ?? "0");
                /*Operador*/
                request.Replace("param_operador_region", Util.IsNotNull(obj.Operador.RegionSolicitud) ?? "");
                request.Replace("param_operador_usuario", Util.IsNotNull(obj.Operador.RunUsuarioSolicitante) ?? "");
                request.Replace("param_operador_empresa", Util.IsNotNull(obj.Operador.RutEmpresaSolicitante) ?? "");
                /*Solicitante*/
                request.Replace("param_solicitante_calidad", Util.IsNotNull(obj.Solicitante.Persona.Calidad) ?? "");
                request.Replace("param_solicitante_rut", Util.IsNotNull(obj.Solicitante.Persona.Rut) ?? "");
                request.Replace("param_solicitante_nombre", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Solicitante.Persona.Nombre)) ?? "");
                request.Replace("param_solicitante_apellido_paterno", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Solicitante.Persona.ApellidoPaterno)) ?? "");
                request.Replace("param_solicitante_apellido_materno", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Solicitante.Persona.ApellidoMaterno)) ?? "");
                request.Replace("param_solicitante_email", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Solicitante.Persona.Email)) ?? "");
                request.Replace("param_solicitante_calle", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Solicitante.Direccion.Calle)) ?? "");
                request.Replace("param_solicitante_direccion", Util.IsNotNull(Util.ConvertSpecialCharacters(obj.Solicitante.Direccion.LetraDireccion)) ?? "");
                request.Replace("param_solicitante_nro_direccion", Util.IsNotNull(obj.Solicitante.Direccion.Numero.ToString()) ?? "");
                request.Replace("param_solicitante_resto_direccion", Util.IsNotNull(obj.Solicitante.Direccion.RestoDomicilio) ?? "");
                request.Replace("param_solicitante_telefono", Util.IsNotNull(obj.Solicitante.Direccion.Telefono.ToString()) ?? "");
                request.Replace("param_solicitante_comuna", Util.IsNotNull(obj.Solicitante.Direccion.Comuna) ?? "");
                request.Replace("param_solicitante_codigo_postal", Util.IsNotNull(obj.Solicitante.Direccion.CodigoPostal) ?? "");
                /*ReIngreso*/
                if (obj.ReIngreso != null)
                {
                    var nodoReIngreso = new StringBuilder(File.ReadAllText(GetTemplateReIngreso));
                    nodoReIngreso.Replace("param_reingreso_ppu", Util.IsNotNull(obj.ReIngreso.PPU) ?? "");
                    nodoReIngreso.Replace("param_reingreso_nro_solicitud", Util.IsNotNull(obj.ReIngreso.NroSolicitud.ToString()) ?? "");
                    nodoReIngreso.Replace("param_reingreso_fecha_solicitud", Util.IsNotNull(obj.ReIngreso.FechaSolicitud.ToString("yyyyMMdd")) ?? "");
                    nodoReIngreso.Replace("param_reingreso_nro_res_exenta", Util.IsNotNull(obj.ReIngreso.NroResolucionExenta.ToString()) ?? "");
                    nodoReIngreso.Replace("param_reingreso_fecha_res_exenta", Util.IsNotNull(obj.ReIngreso.FechaResolucionExenta.ToString("yyyyMMdd")) ?? "");
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
