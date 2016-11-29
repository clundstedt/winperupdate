using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class ModuloBo
    {
        public int idModulo { get; set; }

        public string NomModulo { get; set; }
        public string Descripcion { get; set; }
        public bool isCore { get; set; }
        public string Directorio { get; set; }
        public char Estado { get; set; }
        public string isCoreFmt
        {
            get
            {
                return isCore ? "Si" : "No";
            }
        }
        public string EstadoFmt
        {
            get
            {
                return Estado == 'C' ? "Caducado" : "Vigente";
            }
        }
    }
}
