using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WinperUpdateDAO
{
    public class CnaPerfiles : SpDao
    {
        public SqlDataReader Execute(int idUsuarios)
        {
            SpName = @"SELECT m.* FROM Perfiles_has_Menus pm INNER JOIN Perfiles p
                                    ON pm.CodPrf = p.CodPrf INNER JOIN Menus m
                                    ON pm.idMenus = m.idMenus INNER JOIN Usuarios u
                                    ON p.CodPrf = u.CodPrf
                                 WHERE u.idUsuarios = @idUsuarios
                                    AND m.Orden is not null
                                ORDER BY m.Orden";
            try
            {
                ParmsDictionary.Add("@idUsuarios", idUsuarios);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public new SqlDataReader Execute()
        {
            try
            {
                SpName = @"SELECT * FROM Perfiles";

                return Connector.ExecuteQuery(SpName, null);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
