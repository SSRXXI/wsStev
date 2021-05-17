using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding.ConsultaSolicitud
{
    public class Rechazo
    {
        #region Campos
        private string texto;
        #endregion

        #region Constructor
        public Rechazo() { }
        #endregion

        #region Propiedades
        public string Texto
        {
            get
            {
                return texto;
            }

            set
            {
                texto = value;
            }
        }
        #endregion
    }
}
