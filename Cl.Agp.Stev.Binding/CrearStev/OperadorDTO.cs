using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding.CrearSpie
{
    public class OperadorDTO
    {
        #region Campos
        private string region;
        private string runUsuario;
        private string rEmpresa;
        #endregion

        #region Constructor
        public OperadorDTO() { }
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

        public string REmpresa
        {
            get
            {
                return rEmpresa;
            }

            set
            {
                rEmpresa = value;
            }
        }

        #endregion
    }
}
