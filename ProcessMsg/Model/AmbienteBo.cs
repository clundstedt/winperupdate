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

        public bool EjecutadoOK { get; set; }

        public string NomTipo
        {
            get
            {
                return Tipo == 1 ? "Producción" : "Pruebas";
            }
        }

        public int EstadoEjecucionSql { get; set; }

        public string ColorEstadoEjecucionSql
        {
            get
            {
                string str = "";
                switch (EstadoEjecucionSql)
                {
                    case 0: str = Estado == ' ' ? "warning" : "default";break;
                    case 1: str = "info";break;
                    case 3: str = "danger";break;
                    default:
                        str = "default";
                        break;
                }
                return str;
            }
        }
    }
}
