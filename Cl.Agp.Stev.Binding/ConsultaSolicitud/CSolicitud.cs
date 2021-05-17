using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding.ConsultaSolicitud
{
    public class CSolicitud
    {
        #region Campos
        private string estadoPortal;
        private string estadoSolicitud;
        private List<Rechazo> rechazos;
        #endregion

        #region Constructor
        public CSolicitud() {}
        #endregion

        #region Propiedades
        public string EstadoPortal
        {
            get
            {
                return estadoPortal;
            }

            set
            {
                estadoPortal = value;
            }
        }
        public string EstadoSolicitud
        {
            get
            {
                return estadoSolicitud;
            }

            set
            {
                estadoSolicitud = value;
            }
        }
        public List<Rechazo> Rechazos
        {
            get
            {
                return rechazos;
            }

            set
            {
                rechazos = value;
            }
        }
        #endregion
    }
}
