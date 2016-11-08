using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class TareaBo
    {
        private const int TiempoAtraso = 10;
        public int idTareas { get; set; }
        public int idClientes { get; set; }
        public AmbienteBo Ambientes { get; set; }
        public int CodPrf { get; set; }
        public int Estado { get; set; }
        public string Modulo { get; set; }
        public int idVersion { get; set; }
        public string NameFile { get; set; }
        public string Error { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long LengthFile { get; set; }

        public bool Reportado { get; set; }

        public bool ExisteAtraso
        {
            get
            {
                return (DateTime.Now.Subtract(FechaRegistro).TotalMinutes > TiempoAtraso);
            }
        }

        public string ClassId
        {
            get
            {
                string clas = "";
                if (Estado == 0)
                {
                    clas = "warning";
                }
                else if (Estado == 1)
                {
                    clas = "danger";
                }

                return clas;
            }
        }

        public string EstadoFmt
        {
            get
            {
                string fmt = "";
                switch (Estado)
                {
                    case 0:
                        fmt = "Pendiente";
                        break;
                    case 1:
                        fmt = "Ejecutado";
                        break;
                    default:
                        fmt = "Ejecutado con errores";
                        break;
                }
                return fmt;
            }
        }

        public string FechaRegistroFmt
        {
            get
            {
                return string.Format("{0} {1}/{2}/{3}", FechaRegistro.TimeOfDay, FechaRegistro.Day, FechaRegistro.Month, FechaRegistro.Year);
            }
        }
    }
}
