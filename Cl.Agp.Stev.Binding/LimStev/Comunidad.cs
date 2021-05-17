using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding
{
    public class Comunidad
    {
        #region Campos
        private string cantidad;
        private string esComunidad;
        #endregion

        #region Constructor
        public Comunidad() {}
        #endregion

        #region Propiedades
        public string Cantidad
        {
            get
            {
                return cantidad;
            }

            set
            {
                cantidad = value;
            }
        }

        public string EsComunidad
        {
            get
            {
                return esComunidad;
            }

            set
            {
                esComunidad = value;
            }
        }
        #endregion
    }
}
