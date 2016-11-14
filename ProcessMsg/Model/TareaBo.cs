using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class TareaBo
    {
        //private const int TiempoAtraso = 10; //Expresado en minutos
        public int idTareas { get; set; }
        public int idClientes { get; set; }
        public AmbienteBo Ambientes { get; set; }
        public int CodPrf { get; set; }
        /// <summary>
        /// Estado = 0: Tarea pendiente
        /// Estado = 1: Tarea ejecutada OK
        /// Estado = 2: Tarea ejecutada ERROR
        /// Estado = 3: Tarea ejecutada por el cliente OK
        /// Estado = 4: Tarea ejecutada por el cliente ERROR
        /// </summary>
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
                //return (DateTime.Now.Subtract(FechaRegistro).TotalMinutes > TiempoAtraso);
                return (Estado == 0 || Estado == 2 || Estado == 4);
            }
        }

        public string ClassId
        {
            get
            {
                string clas = "";
                switch (Estado)
                {
                    case 0:
                        clas = "warning";
                        break;
                    case 1:
                        clas = "success";
                        break;
                    case 3:
                        clas = "success";
                        break;
                    default:
                        clas = "danger";
                        break;
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
                    case 2:
                        fmt = "Ejecutado con errores";
                        break;
                    case 3:
                        fmt = "Ejecutado manualmente con exito";
                        break;
                    default:
                        fmt = "Ejecutado manualmente con errores";
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
