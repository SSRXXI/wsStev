using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding
{
    public class Vehiculo
    {
        #region Campos 
        private string patente;
        private string nroMotor;
        private string nroChasis;
        private string nroSerie;
        private string nroVin;
        #endregion

        #region Constructor
        public Vehiculo() { }
        #endregion

        #region Propiedades
        public string Patente
        {
            get
            {
                return patente;
            }

            set
            {
                patente = value;
            }
        }

        public string NroMotor
        {
            get
            {
                return nroMotor;
            }

            set
            {
                nroMotor = value;
            }
        }

        public string NroChasis
        {
            get
            {
                return nroChasis;
            }

            set
            {
                nroChasis = value;
            }
        }

        public string NroSerie
        {
            get
            {
                return nroSerie;
            }

            set
            {
                nroSerie = value;
            }
        }

        public string NroVin
        {
            get
            {
                return nroVin;
            }

            set
            {
                nroVin = value;
            }
        }

        #endregion
    }
}
