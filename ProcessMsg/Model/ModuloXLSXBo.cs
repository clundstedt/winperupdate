using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class ModuloXLSXBo : ModuloBo
    {
        public DateTime FechaRegistro { get; set; }
        public int EstadoRegistro { get; set; }
        public string ErrorRegistro { get; set; }
        public int idUsuario { get; set; }
        public string GetClass
        {
            get
            {
                return EstadoRegistro == 1 ? "danger" : "success";
            }
        }

    }
}
