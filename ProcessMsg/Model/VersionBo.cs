using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessMsg.Model
{
    public class VersionBo
    {
        public int IdVersion { get; set; }
        public string Release { get; set; }
        public DateTime Fecha { get; set; }
        public char Estado { get; set; }
        public string Comentario { get; set; }
        public string Usuario { get; set; }
        public string Instalador { get; set; }
        public long Length { get; set; }
        public bool IsVersionInicial { get; set; }
        public string IsVersionInicialFmt
        {
            get
            {
                return IsVersionInicial ? "Versión full" : "Versión parcial";
            }
        }
        public List<AtributosArchivoBo> Componentes { get; set; }
        public string FechaFmt
        {
            get {
                return string.Format("{0:dd/MM/yyyy}", Fecha);
            }
        }
        public int TotalComponentes
        {
            get
            {
                return Componentes == null ? 0 : Componentes.Count();
            }
        }
        public string EstadoDisplay
        {
            get
            {
                return Estado == 'P' ? "Publicada" : Estado == 'N' ? "Nueva" : "No Vigente";
            }
        }
        public bool HasDeploy31 { get; set; }
        public bool isInstall { get; set; }
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
                    str = string.Format("{0} Insertado", Release);
                    break;
                case 'D':
                    str = string.Format("{0} Eliminado", Release);
                    break;
                case 'U':
                    str = string.Format(@"idVersion={0}|NumVersion={1}|FecVersion={2}|Estado={3}|Comentario={4}|Usuario={5}|Instalador={6}"
                                        , IdVersion
                                        , Release
                                        , FechaFmt
                                        , EstadoDisplay
                                        , Comentario
                                        , Usuario
                                        , Instalador);
                    break;
                case '?':
                    str = string.Format("{0}", Release);
                    break;
                default:
                    break;
            }
            return str;
        }
    }
}
