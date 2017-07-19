using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class TipoComponenteBo
    {
        public TipoComponenteBo()
        {
            LblEliminarTipo = "Eliminar";
        }
        public int idTipoComponentes { get; set; }
        public string Nombre { get; set; }
        public bool isCompBD { get; set; }
        public string IsCompBDFmt
        {
            get
            {
                return isCompBD ? "Si es de Base de Datos" : "NO es de Base de Datos";
            }
        }
        public bool isCompDLL { get; set; }
        public string Extensiones { get; set; }
        public string IsCompDLLFmt
        {
            get
            {
                return isCompDLL ? "Componente DLL":"NO es componente DLL";
            }
        }
        public bool isCompCambios { get; set; }
        public string isCompCambiosFmt
        {
            get
            {
                return isCompCambios ? "Es control de cambios." : "No es control de cambios.";
            }
        }
        public string LblEliminarTipo { get; set; }

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
                    str = string.Format(@"idTipoComponentes={0}|Nombre={1}|isCompBD={2}|isCompDLL={3}|Extensiones={4}"
                                        , idTipoComponentes
                                        , Nombre
                                        , isCompBD
                                        , isCompDLL
                                        , Extensiones);
                    break;
                case '?':
                    str = string.Format("Tipo de Componente: {0}", Nombre);
                    break;
                default:
                    break;
            }
            return str;
        }
    }
}
