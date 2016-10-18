using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaUsuario : SpDao
    {
        public SqlDataReader Execute(string mail)
        {
            SpName = @"select a1.*, a2.Apellidos, a2.Nombres, a2.Mail 
                       from   Usuarios a1, Personas a2 
                       Where  a2.idPersonas = a1.idPersonas
                       And    a2.Mail = @mail";
            try
            {
                ParmsDictionary.Add("@mail", mail);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public new SqlDataReader Execute()
        {
            SpName = @"select a1.idUsuarios, a1.idPersonas, a1.CodPrf, a1.EstUsr, a2.Apellidos, a2.Nombres, a2.Mail 
                       from   Usuarios a1, Personas a2 
                       Where  a1.CodPrf < 10
                       And    a2.idPersonas = a1.idPersonas";
            try
            {
                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader Execute(int codPrf)
        {
            SpName = @"select a1.idUsuarios, a1.idPersonas, a1.CodPrf, a1.EstUsr, a2.Apellidos, a2.Nombres, a2.Mail 
                       from   Usuarios a1, Personas a2 
                       Where  a1.CodPrf > @codPrf
                       And    a2.idPersonas = a1.idPersonas";
            try
            {
                ParmsDictionary.Add("@codPrf", codPrf);

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
