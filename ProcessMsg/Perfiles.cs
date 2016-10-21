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
                        CodPrf = int.Parse(reader["CodPrf"].ToString())
                    });
                }
                return lista;
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
    }
}
