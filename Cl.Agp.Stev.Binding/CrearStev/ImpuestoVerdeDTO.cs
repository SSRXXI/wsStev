using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding.CrearSpie
{
    public class ImpuestoVerdeDTO
    {
        #region Campos
        private string cid;
        private string cit;
        private string mImpuesto;
        private string tFactura;
        #endregion

        #region Constructor
        public ImpuestoVerdeDTO() { }
        #endregion

        #region Propiedades
        public string Cid
        {
            get
            {
                return cid;
            }

            set
            {
                cid = value;
            }
        }

        public string Cit
        {
            get
            {
                return cit;
            }

            set
            {
                cit = value;
            }
        }

        public string MImpuesto
        {
            get
            {
                return mImpuesto;
            }

            set
            {
                mImpuesto = value;
            }
        }

        public string TFactura
        {
            get
            {
                return tFactura;
            }

            set
            {
                tFactura = value;
            }
        }
        #endregion
    }
}
