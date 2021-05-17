using Cl.Agp.Stev.Binding;
using Cl.Agp.Stev.Binding.DTO;
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
    public class AnotacionesVigentesSignature : Template, ISoapCustomRequest<ConsultaAnotacionesVigentesRequest>, ISoapCustomResponse<ConsultaAnotacionesVigentesResponse>
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
                return HttpContext.Current.Server.MapPath(Util.GetValue("template:CCAV"));
            }
        }
        private string GetSoapAction
        {
            get
            {
                return Util.GetValue("SOAPAction:AnotacionesVigentes");
            }
        }
        public ConsultaAnotacionesVigentesResponse Deserialize(XDocument response)
        {
            ConsultaAnotacionesVigentesResponse customResponse = new ConsultaAnotacionesVigentesResponse();
            try
            {
                customResponse.CodigoRespuesta = response.Descendants().Where(n => n.Name == "codigoresp").FirstOrDefault().Value.ToString() ?? "";
                customResponse.Glosa = response.Descendants().Where(n => n.Name == "glosa").FirstOrDefault().Value.ToString() ?? "";

                if (response.XPathSelectElement("//Observaciones") != null)
                {
                    //customResponse.Observaciones = new List<Observacion>();

                    //IEnumerable<XElement> observacionesXML = response.XPathSelectElements("//Observaciones");

                    //XElement po = XElement.Load("//Observaciones");
                    //IEnumerable<XElement> childElements =
                    //    from el in po.Elements()
                    //    select el;
                    //foreach (XElement el in childElements)
                    //    Console.WriteLine("Name: " + el.Name);

                    //foreach (XElement observacionXML in observacionesXML)
                    //{
                    //    string detalle = observacionXML.Element("Observa").Value.ToString();
                    //    if (!string.IsNullOrEmpty(detalle))
                    //    {
                    //        customResponse.Observaciones.Add(
                    //             new Observacion()
                    //             {
                    //                 Detalle = observacionXML.Element("Observa").Value.ToString()
                    //             }
                    //        );
                    //    }
                    //}
                }

                if (customResponse.CodigoRespuesta == "1")
                {
                    var propietarios = response.Descendants().Where(n => n.Name == "datosPropietario").ToArray();
                    customResponse.CertificadoAnotacionesVigentes = new CertificadoAnotacionesVigentes()
                    {
                        ActualPropietario = new PersonaSimpleDTO(
                            propietarios[0].Descendants().Where(n => n.Name == "nombres")?.FirstOrDefault()?.Value.ToString() ?? "",
                            propietarios[0].Descendants().Where(n => n.Name == "apellidoPaterno")?.FirstOrDefault()?.Value.ToString() ?? "",
                            propietarios[0].Descendants().Where(n => n.Name == "apellidoMaterno")?.FirstOrDefault()?.Value.ToString() ?? ""
                        ),
                        vehiculoDTO = new VehiculoDTO(
                            response.Descendants().Where(n => n.Name == "tipo").FirstOrDefault().Value.ToString() ?? "",
                            response.Descendants().Where(n => n.Name == "marca").FirstOrDefault().Value.ToString() ?? "",
                            response.Descendants().Where(n => n.Name == "modelo").FirstOrDefault().Value.ToString() ?? "",
                            int.Parse(response.Descendants().Where(n => n.Name == "aFabrica").FirstOrDefault().Value.ToString()),
                            response.Descendants().Where(n => n.Name == "color").FirstOrDefault().Value.ToString() ?? "",
                            response.Descendants().Where(n => n.Name == "numeroMotor").FirstOrDefault().Value.ToString() ?? "",
                            response.Descendants().Where(n => n.Name == "chasis").FirstOrDefault().Value.ToString() ?? "",
                            response.Descendants().Where(n => n.Name == "numeroSerie").FirstOrDefault().Value.ToString() ?? "",
                            response.Descendants().Where(n => n.Name == "vin").FirstOrDefault().Value.ToString() ?? "",
                            response.Descendants().Where(n => n.Name == "tipoCombustible").FirstOrDefault().Value.ToString() ?? "",
                            response.Descendants().Where(n => n.Name == "pesoBruto").FirstOrDefault().Value.ToString() ?? ""
                        )
                    };
                }
                return customResponse;
            }
            catch (Exception e)
            {
                customResponse.CodigoRespuesta = "-105";
                customResponse.Glosa = "Se ha producido un error al invocar el servicio AnotacionesVigentesStev, Error: " + e;
                log.Error($"()=>, Exception : {e.Message} StackTrace : {e.StackTrace}");
                return customResponse;
            }
            finally
            {
                if (Util.GetValue("SoapTrace").Equals("1"))
                {
                    Util.CapTrace("AnotacionesVigentesStev-", response.ToString());
                }
            }
        }

        public XDocument GenerateQuery(ConsultaAnotacionesVigentesRequest obj)
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
                request.Replace("param_servicio", Util.IsNotNull(obj.Servicio) ?? "");
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
                    Util.CapTrace("AnotacionesVigentesStev-", request.ToString());
                }
            }
        }
    }
}
