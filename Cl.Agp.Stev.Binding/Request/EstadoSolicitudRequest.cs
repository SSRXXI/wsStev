using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Stev.Binding.Request
{
    public class EstadoSolicitudRequest : Request
    {
        public string Ppu { get; set; }
        public string Oficina { get; set; }
        public int NroSolicitud { get; set; }
        public int AnioSolicitud { get; set; }
        
    }
}
