using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaVersionesCliente : SpDao
    {
        public SqlDataReader Execute(int id)
        {
            SpName = @" select a2.* 
                        from   Versiones_has_Clientes a1,
                               Versiones              a2
                        where  a1.idClientes = @id
                        and    a2.idVersion = a1.idVersion";
            try
            {
                ParmsDictionary.Add("@id", id);

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
