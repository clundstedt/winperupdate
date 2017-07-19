using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class ComponenteModuloBo
    {
        public ComponenteModuloBo()
        {
            LblEliminar = "Eliminar";
            LblModificar = "Modificar";
        }
        public int idComponentesModulos { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Modulo { get; set; }
        public TipoComponenteBo TipoComponentes { get; set; }
        public bool VerDescripcion { get; set; }
        public string LblEliminar { get; set; }
        public string LblModificar { get; set; }

        public string Directorio { get; set; }
        public string NomModulo { get; set; }

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
                    str = string.Format(@"Nombre={0}|Descripcion={1}|TipoComponentes={2}"
                                        , Nombre
                                        , Descripcion
                                        , TipoComponentes);
                    break;
                case '?':
                    str = string.Format("{0} (Módulo: {1})", Nombre, NomModulo);
                    break;
                default:
                    break;
            }
            return str;
        }

    }
}
