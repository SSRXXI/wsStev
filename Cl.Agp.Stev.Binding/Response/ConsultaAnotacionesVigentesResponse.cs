using System.Collections.Generic;

namespace Cl.Agp.Stev.Binding.Response
{
    public class ConsultaAnotacionesVigentesResponse : Etapa1Response
    {
        //public List<Observacion> Observaciones { get; set; }
        public Solicitud solicitud { get; set; }
        public CertificadoAnotacionesVigentes CertificadoAnotacionesVigentes { get; set; }
    }
}
