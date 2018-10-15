using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class FunesNotificacionBo
    {
        public string IdSolicitud { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public int Estado { get; set; }

        public string Comentario { get; set; }

        public string Token { get; set; }

        public List<FunesEmpresaBo> Funes { get; set; }
    }
}
