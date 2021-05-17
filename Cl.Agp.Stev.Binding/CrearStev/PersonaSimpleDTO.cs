namespace Cl.Agp.Stev.Binding.DTO
{
    public class PersonaSimpleDTO
    {
        public string Nombre { get; private set; }
        public string ApellidoPaterno { get; private set; }
        public string ApellidoMaterno { get; private set; }

        public PersonaSimpleDTO(string nombre,string apellidoPaterno,string apellidoMaterno)
        {
            Nombre = nombre;
            ApellidoPaterno = apellidoPaterno;
            ApellidoMaterno = apellidoMaterno;
        }

        public PersonaSimpleDTO() { }
    }
}
