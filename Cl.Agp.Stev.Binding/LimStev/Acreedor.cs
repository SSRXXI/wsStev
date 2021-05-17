using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding
{
    public class Acreedor
    {
        #region Campos
        private string runRut;
        private string nombreRazon;
        #endregion

        #region Constructor
        public Acreedor() { }
        #endregion

        #region Propiedades
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

        public string NombreRazon
        {
            get
            {
                return nombreRazon;
            }

            set
            {
                nombreRazon = value;
            }
        }
        #endregion 


    }
}
