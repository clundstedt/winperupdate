using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaUsuarioById : SpDao
    {
        public SqlDataReader Execute(int id)
        {
            SpName = @"select a1.idUsuarios, a1.idPersonas, a1.CodPrf, a5.nombre as NombrePerfil, a1.EstUsr, a2.Apellidos, a2.Nombres, a2.Mail, a4.idClientes 
                        from   Usuarios a1
                        join   Personas a2 
                        on     a2.idPersonas = a1.idPersonas
                        left join Clientes_has_Usuarios a3
                        on     a3.idUsuarios = a1.idUsuarios
                        left join Clientes a4
                        on     a4.idClientes = a3.idClientes
                        join Perfiles a5
						on a1.CodPrf = a5.CodPrf
                        Where  a1.idUsuarios = @id";
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
