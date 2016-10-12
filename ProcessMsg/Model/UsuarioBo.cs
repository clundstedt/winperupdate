using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class UsuarioBo
    {
        public int Id { get; set; }

        public string Clave { get; set; }

        public int CodPrf { get; set; }

        public PersonaBo Persona { get; set; }

        public char EstUsr { get; set; }

        public string EstadoDisplay
        {
            get
            {
                if (EstUsr == 'V') return "Vigente";
                if (EstUsr == 'C') return "Caduco";
                return "";
            }
        }
    }
}
