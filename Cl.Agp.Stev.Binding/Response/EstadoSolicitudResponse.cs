using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Stev.Binding.Response
{
    public class EstadoSolicitudResponse : Etapa1Response
    {
        public Solicitud Solicitud { get; set; }
        public Rechazos Rechazos { get; set; }

    }
}
