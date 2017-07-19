using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class ModuloBo
    {
        public int idModulo { get; set; }

        public string NomModulo { get; set; }
        public string Descripcion { get; set; }
        public bool isCore { get; set; }
        public string Directorio { get; set; }
        public char Estado { get; set; }
        public string isCoreFmt
        {
            get
            {
                return isCore ? "Este módulo es parte del sistema principal." : "Este módulo no es parte del sistema principal.";
            }
        }
        public string EstadoFmt
        {
            get
            {
                return Estado == 'C' ? "No Vigente" : "Vigente";
            }
        }
        public int Suite { get; set; }
        public List<ComponenteModuloBo> ComponentesModulo { get; set; }

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
                    str = string.Format("{0} Insertado", NomModulo);
                    break;
                case 'D':
                    str = string.Format("{0} Eliminado", NomModulo);
                    break;
                case 'U':
                    str = string.Format(@"NomModulo={0}|Descripcion={1}|isCore={2}|Directorio={3}|Suite={4}"
                                        , NomModulo
                                        , Descripcion
                                        , isCore
                                        , Directorio
                                        , Suite);
                    break;
                case '?':
                    str = string.Format("{0}", NomModulo);
                    break;
                default:
                    break;
            }
            return str;
        }
    }
}
