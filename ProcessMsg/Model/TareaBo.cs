using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class TareaBo
    {
        public int idTareas { get; set; }
        public int idClientes { get; set; }
        public AmbienteBo Ambientes { get; set; }
        public int CodPrf { get; set; }
        public int Estado { get; set; }
        public string Modulo { get; set; }
        public int idVersion { get; set; }
        public string NameFile { get; set; }
        public string Error { get; set; }
    }
}
