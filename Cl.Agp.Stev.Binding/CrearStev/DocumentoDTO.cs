using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding.CrearSpie
{
    public class DocumentoDTO
    {
        #region Campos
        private string emisor;
        private string fecha;
        private string lugar;
        private string mbTotal;
        private string numero;
        private string tipo;
        private string rEmisor;
        private string tMoneda;
        #endregion

        #region Constructor
        public DocumentoDTO() { }
        #endregion

        #region Propiedades
        public string Emisor
        {
            get
            {
                return emisor;
            }

            set
            {
                emisor = value;
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

        public string MbTotal
        {
            get
            {
                return mbTotal;
            }

            set
            {
                mbTotal = value;
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

        public string Tipo
        {
            get
            {
                return tipo;
            }

            set
            {
                tipo = value;
            }
        }

        public string REmisor
        {
            get
            {
                return rEmisor;
            }

            set
            {
                rEmisor = value;
            }
        }

        public string TMoneda
        {
            get
            {
                return tMoneda;
            }

            set
            {
                tMoneda = value;
            }
        }

        #endregion
    }
}
