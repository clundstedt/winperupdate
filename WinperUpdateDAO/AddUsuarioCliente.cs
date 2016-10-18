using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddUsuarioCliente : SpDao
    {
        public int Execute(int idUsuario, int idCliente)
        {
            SpName = @" insert into Clientes_has_Usuarios (idClientes, idUsuarios) 
                                      values (@idCliente, @idUsuario)";
            try
            {
                ParmsDictionary.Add("@idCliente", idCliente);
                ParmsDictionary.Add("@idUsuario", idUsuario);

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
