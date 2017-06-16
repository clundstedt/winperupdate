using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class ClienteBo
    {
        public int Id { get; set; }

        public int Rut { get; set; }

        public char Dv { get; set; }

        public string Nombre { get; set; }

        public string Direccion { get; set; }

        public ComunaBo Comuna { get; set; }

        public string NroLicencia { get; set; }

        public string RutFmt
        {
            get
            {
                return string.Format("{0}-{1}", Rut, Dv);
            }
        }

        public int NumFolio { get; set; }
        
        public int EstMtc { get; set; }
        public string Mesini { get; set; }
        public string NroTrbc { get; set; }
        public string NroTrbh { get; set; }
        public string NroUsr { get; set; }

        public string MesCon { get; set; }
        public int Correlativo { get; set; }
        public string EstMtcFmt
        {
            get
            {
                return EstMtc == 7 ? "Activo" : "No Activo";
            }
        }

        public string MesIniFmt
        {
            get
            {
                var nom = "";
                switch (Mesini)
                {
                    case "01":
                        nom = "Enero";
                        break;
                    case "02":
                        nom = "Febrero";
                        break;
                    case "03":
                        nom = "Marzo";
                        break;
                    case "04":
                        nom = "Abril";
                        break;
                    case "05":
                        nom = "Mayo";
                        break;
                    case "06":
                        nom = "Junio";
                        break;
                    case "07":
                        nom = "Julio";
                        break;
                    case "08":
                        nom = "Agosto";
                        break;
                    case "09":
                        nom = "Septiembre";
                        break;
                    case "10":
                        nom = "Octubre";
                        break;
                    case "11":
                        nom = "Noviembre";
                        break;
                    default: nom = "Diciembre";
                        break;
                }
                return nom;
            }
        }

        public char Estado { get; set; }

        public string EstadoFmt
        {
            get
            {
                return Estado == 'C' ? "No Vigente" : "Vigente";
            }
        }
    }
}
