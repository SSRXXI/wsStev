using System;

namespace Cl.Agp.Stev.Binding
{
    public class Documento
    {
        public string TipoDocumento { get; set; }
        public string Naturaleza { get; set; }
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string Lugar { get; set; }
        public long Total { get; set; }
        public string Moneda { get; set; }
        public string Kilometraje { get; set; }
        public string RutEmisor { get; set; }
        public string CodigoNotaria { get; set; }
        public string Autorizante { get; set; }
        public string Observacion { get; set; }
    }
}
