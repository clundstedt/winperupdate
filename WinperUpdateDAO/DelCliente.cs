using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class DelCliente : SpDao
    {
        public int Execute(int id, char est)
        {
            SpName = @"update Clientes set Estado = @est where idClientes = @id";
            try
            {
                ParmsDictionary.Add("@id", id);
                ParmsDictionary.Add("@est", est);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
