using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class AmbientesXLSXBo
    {
        public int idAmbientes { get; set; }
        public int idClientes { get; set; }
        public string Nombre { get; set; }
        public int? Tipo { get; set; }
        public string ServerBd { get; set; }
        public string Instancia { get; set; }
        public string NomBd { get; set; }
        public string UserDbo { get; set; }
        public string PwdDbo { get; set; }
        public DateTime FechaRegistro
        {
            get
            {
                return DateTime.Now;
            }
        }
        public DateTime FechaRegistroEx { get; set; }
        public int EstadoRegistro { get; set; }
        public string ErrorRegistro { get; set; }

        public string GetClass
        {
            get
            {
                return EstadoRegistro == 1 ? "danger" : "success";
            }
        }

    }
}
