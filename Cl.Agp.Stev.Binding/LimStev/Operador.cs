using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding
{
    public class Operador
    {
        #region Campos
        private string region;
        private string runUsuario;
        private string empresa;
        #endregion

        #region Constructor
        public Operador() { }
        #endregion

        #region Propiedades
        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
            }
        }

        public string RunUsuario
        {
            get
            {
                return runUsuario;
            }

            set
            {
                runUsuario = value;
            }
        }

        public string Empresa
        {
            get
            {
                return empresa;
            }

            set
            {
                empresa = value;
            }
        }

        #endregion
    }
}
