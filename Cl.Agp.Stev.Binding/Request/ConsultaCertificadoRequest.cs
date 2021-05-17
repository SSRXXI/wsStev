namespace Cl.Agp.Stev.Binding.Request
{
    public class ConsultaCertificadoRequest : Request
    {
        public int NroSolicitud { get; set; }
        public int AnioSolicitud { get; set; }
        public string Ppu { get; set; }
    }
}
