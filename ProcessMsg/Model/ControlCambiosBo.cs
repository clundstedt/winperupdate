using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class ControlCambiosBo
    {
        public int Version { get; set; }
        public int Modulo { get; set; }
        public string Release { get; set; }
        public int Tips { get; set; }
        public string Descripcion { get; set; }
        public List<string> DocCambios { get; set; }
        public DateTime Fecha { get; set; }
        public string Impacto { get; set; }

        public string FechaFmt {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", Fecha);
            }
        }
        public string Usuario { get; set; }
        public ModuloBo ModuloFmt { get; set; }
        public VersionBo VersionFmt { get; set; }


        public string DocCambiosFmt
        {
            get
            {
                return DocCambios == null ? "Sin Documentos" : string.Join(", ", DocCambios);
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
                    str = string.Format("Control de Cambios Insertado en Versión {0}", VersionFmt.Release);
                    break;
                case 'D':
                    str = string.Format("Control de Cambios Eliminado en Versión {0}", VersionFmt.Release);
                    break;
                case 'U':
                    str = string.Format(@"Tips={0}|Version={1}|Modulo={2}|Release={3}|Descripcion={4}|Fecha={5}|Impacto={6}|Documentos={7}"
                                        , Tips
                                        , Version
                                        , Modulo
                                        , Release
                                        , Descripcion
                                        , FechaFmt
                                        , Impacto
                                        , DocCambiosFmt);
                    break;
                case '?':
                    str = string.Format("Control Cambios en Versión {0}, Tips {1}", VersionFmt.Release, Tips);
                    break;
                default:
                    break;
            }
            return str;
        }
        
    }
}
