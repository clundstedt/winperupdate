using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class FunesEmpresaBo
    {
        public string rutEmpresa { get; set; }

        public FunesUnidadGestionBo unidadGestion { get; set; }

        public List<FunesTrabajadorBo> trabajadores { get; set; }
    }
}
