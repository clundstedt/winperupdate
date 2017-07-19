using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaBitacora : SpDao
    {
        public SqlDataReader Execute(string menu)
        {
            try
            {
                SpName = @"SELECT TOP 200 * FROM Bitacora WHERE menu = @menu ORDER BY fecha DESC";

                ParmsDictionary.Add("@menu", menu);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader Execute(int usuario)
        {
            try
            {
                SpName = @"SELECT TOP 200 * FROM Bitacora WHERE usuario = @usuario ORDER BY fecha DESC";
                ParmsDictionary.Add("@usuario", usuario);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
