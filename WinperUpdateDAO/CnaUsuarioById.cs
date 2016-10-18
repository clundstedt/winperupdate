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
            SpName = @"select a1.idUsuarios, a1.idPersonas, a1.CodPrf, a1.EstUsr, a2.Apellidos, a2.Nombres, a2.Mail 
                       from   Usuarios a1, Personas a2 
                       Where  a1.idUsuarios = @id
                       And    a2.idPersonas = a1.idPersonas";
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
