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
                if (EstUsr == 'C') return "Caduco";
                return "";
            }
        }
        
        public string NombrePerfil
        {
            get
            {
                object[,] mz =
                {
                    { 1, "Administrador" },
                    { 2, "Desarrollador" },
                    { 3, "Soporte" },
                    { 4, "Gestión" },
                    { 11, "Administrador" },
                    { 12, "DBA" }
                };
                #pragma warning disable
                for (int x = 0; x < mz.Length; x++)
                {
                    return (int.Parse(mz[x, 0].ToString()) == CodPrf) ? mz[x, 1].ToString() : null;
                }
                return null;
            }
        }

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
    }
}
