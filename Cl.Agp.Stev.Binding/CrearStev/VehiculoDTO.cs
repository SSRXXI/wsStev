namespace Cl.Agp.Stev.Binding.DTO
{
    public class VehiculoDTO
    {
        public string TipoVehiculo { get; private set; }
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public int AnioFabricacion { get; private set; }
        public string Color { get; private set; }
        public string NroMotor { get; private set; }
        public string Chasis { get; private set; }
        public string NroSerie { get; private set; }
        public string NroVin { get; private set; }
        public string TipoCombustible { get; private set; }
        public string PesoBruto { get; private set; }

        public VehiculoDTO(string tipoVehiculo,
                           string marca,
                           string modelo,
                           int anioFabricacion,
                           string color,
                           string nroMotor,
                           string chasis,
                           string nroSerie,
                           string nroVin,
                           string tipoCombustible,
                           string pesoBruto)
        {


            TipoVehiculo = tipoVehiculo;
            Marca = marca;
            Modelo = modelo;
            AnioFabricacion = anioFabricacion;
            Color = color;
            NroMotor = nroMotor;
            Chasis = chasis;
            NroSerie = nroSerie;
            NroVin = NroVin;
            TipoCombustible = tipoCombustible;
            PesoBruto = pesoBruto;
        }

        public VehiculoDTO() { }
     
    }
}
