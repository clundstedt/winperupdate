using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaClienteUsuario : SpDao
    {
        public SqlDataReader Execute(int id)
        {
            SpName = @" select a2.*
                        from   Clientes_has_Usuarios a1,
                               Clientes              a2
                        where  a1.idUsuarios = @id
                        and    a2.idClientes = a1.idClientes";
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
