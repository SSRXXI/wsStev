using System.Collections.Generic;

namespace Cl.Agp.Stev.Binding.Response
{
    public class CreateLimitStevResponse : Etapa1Response
    {
        public Solicitud Solicitud { get; set; }
        public List<Observacion> Observaciones { get; set; }
    }
}
