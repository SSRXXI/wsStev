using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding.CrearSpie
{
    public class SolicitanteDTO
    {
        #region Campos 
        private string calidad;
        private string calle;
        private string comuna;
        private string email;
        private string ltrDomicilio;
        private string nombresRazon;
        private string nroDomicilio;
        private string runRut;
        private string telefono;
        private string aMaterno;
        private string aPaterno;
        private string cPostal;
        private string rDomicilio;
        #endregion

        #region Constructor
        public SolicitanteDTO() { }
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

        public string Calle
        {
            get
            {
                return calle;
            }

            set
            {
                calle = value;
            }
        }

        public string Comuna
        {
            get
            {
                return comuna;
            }

            set
            {
                comuna = value;
            }
        }

        public string LtrDomicilio
        {
            get
            {
                return ltrDomicilio;
            }

            set
            {
                ltrDomicilio = value;
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

        public string NroDomicilio
        {
            get
            {
                return nroDomicilio;
            }

            set
            {
                nroDomicilio = value;
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

        public string Telefono
        {
            get
            {
                return telefono;
            }

            set
            {
                telefono = value;
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

        public string CPostal
        {
            get
            {
                return cPostal;
            }

            set
            {
                cPostal = value;
            }
        }

        public string RDomicilio
        {
            get
            {
                return rDomicilio;
            }

            set
            {
                rDomicilio = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        #endregion
    }
}
