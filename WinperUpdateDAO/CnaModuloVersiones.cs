using System;
using System.Data.SqlClient;

namespace WinperUpdateDAO
{
    public class CnaModuloVersiones : SpDao
    {
        public SqlDataReader Execute(int idVersion)
        {
            SpName = @" select * from ModuloVersiones where idVersion = @idVersion";
            try
            {
                ParmsDictionary.Add("@idVersion", idVersion);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
