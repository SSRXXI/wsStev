using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Spiev.Binding.LimSpie
{
    public class Solicitud
    {
        #region Campos 
        private string fecha;
        private string hora;
        private string numeroSol;
        private string oficina;
        private string ppu;
        private string tipoSol;
        #endregion

        #region Constructor 
        public Solicitud() { }
        #endregion

        #region Propiedades
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

        public string Hora
        {
            get
            {
                return hora;
            }

            set
            {
                hora = value;
            }
        }

        public string NumeroSol
        {
            get
            {
                return numeroSol;
            }

            set
            {
                numeroSol = value;
            }
        }

        public string Oficina
        {
            get
            {
                return oficina;
            }

            set
            {
                oficina = value;
            }
        }

        public string Ppu
        {
            get
            {
                return ppu;
            }

            set
            {
                ppu = value;
            }
        }

        public string TipoSol
        {
            get
            {
                return tipoSol;
            }

            set
            {
                tipoSol = value;
            }
        }
        #endregion

    }
}
