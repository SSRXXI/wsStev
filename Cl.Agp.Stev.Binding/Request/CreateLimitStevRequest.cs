using System.Collections.Generic;

namespace Cl.Agp.Stev.Binding.Request
{
    public class CreateLimitStevRequest : Request
    {
        public Propietario Propietario { get; set; }
        public List<Propietario> propietariosComunidad { get; set; }
        public Acreedor Acreedor { get; set; }
        public List<Acreedor> Acreedores { get; set; }
        public Documento Documento { get; set; }
        public Operador Operador { get; set; }
        public Solicitante Solicitante { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public ReIngreso Reingreso { get; set; }
    }
}
