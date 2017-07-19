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

        /// <summary>
        /// Retorna información para el registro de la bitacora.
        /// </summary>
        /// <param name="accion">I: Insertado, D: Eliminado, U: Modificado y ?: Dato de Registro</param>
        /// <returns></returns>
        public string Bitacora(char accion)
        {
            string str = "";
            switch (accion)
            {
                case 'I':
                    str = string.Format("{0} Insertado", Nombre);
                    break;
                case 'D':
                    str = string.Format("{0} Eliminado", Nombre);
                    break;
                case 'U':
                    str = string.Format(@"Id={0}|Rut={1}|Dv={2}|Nombre={3}|Direccion={4}|Comuna={5}|NroLicencia={6}|NumFolio={7}|EstMtc={8}|Mesini={9}|NroTrbc={10}|NroTrbh={11}|NroUsr={12}|MesCon={13}|Correlativo={14}"
                                        , Id
                                        , Rut
                                        , Dv
                                        , Nombre
                                        , Direccion
                                        , Comuna.idCmn
                                        , NroLicencia
                                        , NumFolio
                                        , EstMtc
                                        , Mesini
                                        , NroTrbc
                                        , NroTrbh
                                        , NroUsr
                                        , MesCon
                                        , Correlativo);
                    break;
                case '?':
                    str = string.Format("{0}", Nombre);
                    break;
                default:
                    break;
            }
            return str;
        }
    }
}
