using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding
{
    public class Documento
    {
        #region Campos
        private string tipoDoc;
        private string numero;
        private string lugar;
        private string fecha;
        private string autorizante;
        #endregion

        #region Constructor
        public Documento() { }
        #endregion

        #region Propiedades
        public string TipoDoc
        {
            get
            {
                return tipoDoc;
            }

            set
            {
                tipoDoc = value;
            }
        }

        public string Numero
        {
            get
            {
                return numero;
            }

            set
            {
                numero = value;
            }
        }

        public string Lugar
        {
            get
            {
                return lugar;
            }

            set
            {
                lugar = value;
            }
        }

        public string Fecha
        {
            get
            {
                return fecha;
            }

            set
            {
                fecha = value;
            }
        }

        public string Autorizante
        {
            get
            {
                return autorizante;
            }

            set
            {
                autorizante = value;
            }
        }

        #endregion

    }
}
