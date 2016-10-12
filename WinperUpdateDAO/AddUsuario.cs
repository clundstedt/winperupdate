using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddUsuario : SpDao
    {
        public int Execute(int idPersona, int codPrf, string clave, char estado)
        {
            SpName = @" insert into Usuarios (idPersonas, CodPrf, Clave, EstUsr) 
                                      values (@idPersona, @codPrf, @clave, @estado)";
            try
            {
                ParmsDictionary.Add("@idPersona", idPersona);
                ParmsDictionary.Add("@codPrf", codPrf);
                ParmsDictionary.Add("@clave", clave);
                ParmsDictionary.Add("@estado", estado);

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
