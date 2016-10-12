using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class UpdPersona : SpDao
    {
        public int Execute(int idPersona, string apellidos, string nombres, string mail)
        {
            SpName = @" update Personas 
                           set Apellidos = @apellidos, 
                               Nombres = @nombres, 
                               Mail = @mail
                         where idPersonas = @idPersona";
            try
            {
                ParmsDictionary.Add("@apellidos", apellidos);
                ParmsDictionary.Add("@nombres", nombres);
                ParmsDictionary.Add("@mail", mail);
                ParmsDictionary.Add("@idPersona", idPersona);

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
