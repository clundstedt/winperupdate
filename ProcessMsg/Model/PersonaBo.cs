using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class PersonaBo
    {
        public int Id { get; set; }

        public string Apellidos { get; set; }

        public string Nombres { get; set; }

        public string Mail { get; set; }

        public string NomFmt { get
            {
                return Nombres.Trim() + " " + Apellidos.Trim();
            }
        }
    }
}
