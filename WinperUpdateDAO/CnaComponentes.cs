using System;
using System.Data.SqlClient;

namespace WinperUpdateDAO
{
    public class CnaComponentes : SpDao
    {
        public SqlDataReader Execute(int idVersion, string idModulo)
        {
            SpName = @" select * from Componentes where idVersion = @idVersion and Modulo = @idModulo";
            try
            {
                ParmsDictionary.Add("@idVersion", idVersion);
                ParmsDictionary.Add("@idModulo", idModulo);

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
