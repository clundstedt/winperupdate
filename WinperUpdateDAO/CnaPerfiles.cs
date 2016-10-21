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
            SpName = @"SELECT m.* 
                              FROM usuarios u INNER JOIN perfiles p
                              ON u.CodPrf=p.CodPrf INNER JOIN menus m
                              ON p.CodPrf = m.CodPrf
                              WHERE u.idUsuarios = @idUsuarios";
            try
            {
                ParmsDictionary.Add("@idUsuarios", idUsuarios);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
