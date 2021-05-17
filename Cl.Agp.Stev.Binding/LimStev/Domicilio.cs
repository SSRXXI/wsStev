using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding
{
    public class Domicilio
    {
        #region Campos 
        private string calle;
        private string ltrDomicilio;
        private string nroDomicilio;
        private string rDomicilio;
        private string telefono;
        private string comuna;
        private string cPostal;
        #endregion

        #region Constructor
        public Domicilio() { }
        #endregion

        #region Propiedades 
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
            #endregion
        }
}
