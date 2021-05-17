using System;

namespace Cl.Agp.Stev.Binding.Request
{
    public class CargaDocumentoRequest : Request
    {
        public string File { get; set; }
        public string Nombre { get; set; }
        public string Patente { get; set; }
        public string Nro { get; set; }
        public string TipoSolicitud { get; set; }
        public string TipoDocumento { get; set; }
        public string Clasificacion { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
