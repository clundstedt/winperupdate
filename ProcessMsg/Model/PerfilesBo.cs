using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class PerfilesBo
    {
        public int CodPrf { get; set; }
        public string Nombre { get; set; }
        public char Tipo { get; set; }
        public MenusBo Menus { get; set; }
        public string NombreTipo
        {
            get
            {
                return (Tipo == 'I') ? "Interno" : "Externo";
            }
        }
    }
}
