using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaUsuariosCliente : SpDao
    {
        public SqlDataReader Execute(int idCliente)
        {
            SpName = @"select a1.idUsuarios, a1.idPersonas, a1.CodPrf, a1.EstUsr, a2.Apellidos, a2.Nombres, a2.Mail 
                       from   Usuarios a1, Personas a2, Clientes_has_Usuarios a3 
                       Where  a2.idPersonas = a1.idPersonas
                       and    a3.idClientes = @idCliente
                       and    a3.idUsuarios = a1.idUsuarios";
            try
            {
                ParmsDictionary.Add("@idCliente", idCliente);

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
