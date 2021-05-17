using System;

namespace Cl.Agp.Stev.Binding.Response
{
    public class CargaDocumentoResponse : Etapa1Response
    {
        public string Pantentes { get; set; }
        public string Nro { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Nombre { get; set; }
        public string Output { get; set; }
    }
}
