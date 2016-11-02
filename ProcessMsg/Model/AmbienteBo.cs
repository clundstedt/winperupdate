using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class AmbienteBo
    {
        public int idAmbientes { get; set; }
        public int idClientes { get; set; }
        public string Nombre { get; set; }
        public int Tipo { get; set; }
        public string ServerBd { get; set; }
        public string Instancia { get; set; }
        public string NomBd { get; set; }
        public string UserDbo { get; set; }
        public string PwdDbo { get; set; }
        public char Estado { get; set; }

        public string NomTipo
        {
            get
            {
                return Tipo == 1 ? "Producción" : "Desarrollo";
            }
        }
    }
}
