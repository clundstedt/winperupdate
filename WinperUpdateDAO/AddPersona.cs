using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddPersona : SpDao
    {
        public int Execute(string nombre, string apellido, string mail)
        {
            SpName = @" insert into Personas (Nombres, Apellidos, Mail) 
                                      values (@nombre, @apellido, @mail)";
            try
            {
                ParmsDictionary.Add("@nombre", nombre);
                ParmsDictionary.Add("@apellido", apellido);
                ParmsDictionary.Add("@mail", mail);

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
