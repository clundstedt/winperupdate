using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class ClienteNoVigenteBo
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
        public int Cliente { get; set; }
        public int Usuario { get; set; }
        public UsuarioBo UsuarioFmt { get; set; }
        public string FechaFmt
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", Fecha);
            }
        }
    }
}
