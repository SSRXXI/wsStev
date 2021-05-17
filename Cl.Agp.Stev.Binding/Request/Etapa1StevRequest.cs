using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Agp.Stev.Binding.Request
{
    public class Etapa1StevRequest
    {
        public ConsultaAnotacionesVigentesRequest consultaAnotacionesVigentesRequest = new ConsultaAnotacionesVigentesRequest();
        public CreateStevRequest createStevRequest = new CreateStevRequest();
        public CargaDocumentoRequest cargaDocumentoRequest = new CargaDocumentoRequest();
        public CreateLimitStevRequest createLimitStevRequest = new CreateLimitStevRequest();
    }
}
