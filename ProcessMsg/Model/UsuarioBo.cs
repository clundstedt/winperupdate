using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class UsuarioBo
    {
        public int Id { get; set; }

        public string Clave { get; set; }

        public int CodPrf { get; set; }

        public PersonaBo Persona { get; set; }

        public ClienteBo Cliente { get; set; }

        public char EstUsr { get; set; }

        public string EstadoDisplay
        {
            get
            {
                if (EstUsr == 'V') return "Vigente";
                if (EstUsr == 'C') return "No Vigente";
                return "";
            }
        }
        
        public string NombrePerfil { get; set; }

        public string TipoUsuario
        {
            get
            {
                return (Cliente == null) ? "WinPer" : "EMPRESA";
            }
        }

        public string Empresa
        {
            get
            {
                return (Cliente == null) ? "INNOVASOFT" : Cliente.Nombre;
            }
        }

        public string GetToken
        {
           get
            {
                return Utils.G_Encripta(string.Format("{0}:{1}:{2}:{3}:{4:dd-MM-yyyy}",Id, Persona.Id, Persona.Mail,Clave,DateTime.Now));
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
                    str = string.Format("{0} Insertado", Persona.NomFmt);
                    break;
                case 'D':
                    str = string.Format("{0} Eliminado", Persona.NomFmt);
                    break;
                case 'U':
                    str = string.Format(@"idUsuarios={0}|CodPrf={1}|Nombres={2}|Apellidos={3}|Mail={4}|Estado={5}"
                                        , Id
                                        , CodPrf
                                        , Persona.Nombres
                                        , Persona.Apellidos
                                        , Persona.Mail
                                        , EstadoDisplay);
                    break;
                case '?':
                    str = Cliente == null ? string.Format("{0}", Persona.NomFmt) : string.Format("{0} (Cliente: {1})", Persona.NomFmt, Cliente.Nombre);
                    break;
                default:
                    break;
            }
            return str;
        }
    }
}
