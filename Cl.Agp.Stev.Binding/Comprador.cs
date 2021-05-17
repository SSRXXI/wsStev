using System.Collections.Generic;

namespace Cl.Agp.Stev.Binding
{
    public class Comprador
    {
        public Comunidad Comunidad { get; set; }
        public List<Compran> ListadoCompradores { get; set; }
    }
    public class Compran
    {
        public Persona Persona { get; set; }
        public Direccion Direccion { get; set; }
    }
}
