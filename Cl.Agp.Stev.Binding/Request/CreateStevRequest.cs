namespace Cl.Agp.Stev.Binding.Request
{
    public class CreateStevRequest : Request
    {
        public Vehiculo Vehiculo { get; set; }
        public Vendedor Vendedor { get; set; }
        public Comprador Comprador { get; set; }
        public Estipulante Estipulante { get; set; }
        public Documento Documento { get; set; }
        public Impuesto Impuesto { get; set; }
        public Operador Operador { get; set; }
        public Solicitante Solicitante { get; set; }
        public ReIngreso ReIngreso { get; set; }
        
    }
}
