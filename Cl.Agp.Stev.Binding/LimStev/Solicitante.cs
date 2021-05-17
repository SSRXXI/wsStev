using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding
{
    public class Solicitante
    {
        #region Campos 
        private string calidad;
        private string runRut;
        private string nombresRazon;
        private string aPaterno;
        private string aMaterno;
        private Domicilio domicilio;
        #endregion

        #region Constructor
        public Solicitante() { }
        #endregion

        #region Propiedades

        public string Calidad
        {
            get
            {
                return calidad;
            }

            set
            {
                calidad = value;
            }
        }

        public string RunRut
        {
            get
            {
                return runRut;
            }

            set
            {
                runRut = value;
            }
        }

        public string NombresRazon
        {
            get
            {
                return nombresRazon;
            }

            set
            {
                nombresRazon = value;
            }
        }

        public string APaterno
        {
            get
            {
                return aPaterno;
            }

            set
            {
                aPaterno = value;
            }
        }

        public string AMaterno
        {
            get
            {
                return aMaterno;
            }

            set
            {
                aMaterno = value;
            }
        }

        public Domicilio Domicilio
        {
            get
            {
                return domicilio;
            }

            set
            {
                domicilio = value;
            }
        }
        #endregion
    }
}
