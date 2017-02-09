using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddVersionCliente : SpDao
    {
        public int Execute(int idVersion, int idClientes)
        {
            SpName = @" insert into versiones_has_clientes (idVersion, idClientes) 
                                      values (@idVersion, @idClientes)";
            try
            {
                ParmsDictionary.Add("@idVersion", idVersion);
                ParmsDictionary.Add("@idClientes", idClientes);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader Execute(string xml)
        {
            try
            {
                SpName = @"EXEC sp_AddClientesToVersion @xml";

                ParmsDictionary.Add("@xml", xml);
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
