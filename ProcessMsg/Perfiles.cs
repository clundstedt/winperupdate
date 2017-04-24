using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Perfiles
    {
        public static List<Model.MenusBo> GetMenus(int idUsuarios)
        {
            List<Model.MenusBo> lista = new List<Model.MenusBo>();
            try
            {
                var reader = new CnaPerfiles().Execute(idUsuarios);
                while (reader.Read())
                {
                    lista.Add(new Model.MenusBo
                    {
                        idMenus = int.Parse(reader["idMenus"].ToString()),
                        Descripcion = reader["Descripcion"].ToString(),
                        Link = reader["Link"].ToString(),
                        CodPrf = int.Parse(reader["CodPrf"].ToString()),
                        Icon = reader["Icon"].ToString(),
                        Submenus=reader["Submenus"].ToString()
                    });
                }
                reader.Close();
                return lista;
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static List<Model.PerfilesBo> GetPerfiles()
        {
            try
            {
                List<Model.PerfilesBo> lista = new List<Model.PerfilesBo>();
                var res = new CnaPerfiles().Execute();
                while (res.Read())
                {
                    lista.Add(new Model.PerfilesBo
                    {
                        CodPrf = int.Parse(res["CodPrf"].ToString()),
                        Nombre = res["Nombre"].ToString(),
                        Tipo = char.Parse(res["Tipo"].ToString())
                    });
                }
                res.Close();
                return lista;
            }
            catch( Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static Model.PerfilesBo GetPerfil(int CodPrf)
        {
            try
            {
                return GetPerfiles().SingleOrDefault(x => x.CodPrf == CodPrf);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
    }
}
