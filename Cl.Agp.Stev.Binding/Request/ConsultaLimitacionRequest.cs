using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Stev.Binding.Request
{
    public class ConsultaLimitacionRequest : Request
    {
        public int NroSolicitud { get; set; }
        public int AnioSolicitud { get; set; }
        public string Ppu { get; set; }
    }
}

