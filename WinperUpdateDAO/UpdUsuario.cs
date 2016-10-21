using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class UpdUsuario : SpDao
    {
        public int Execute(int id, int idPersona, int codPrf, char estado)
        {
            SpName = @" update Usuarios 
                           set idPersonas = @idPersona, 
                               CodPrf = @codPrf, 
                               EstUsr = @estado
                         where idUsuarios = @id";
            try
            {
                ParmsDictionary.Add("@idPersona", idPersona);
                ParmsDictionary.Add("@codPrf", codPrf);
                ParmsDictionary.Add("@estado", estado);
                ParmsDictionary.Add("@id", id);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int Execute(int id, string pwdNueva)
        {
            SpName = @"UPDATE usuarios SET clave = @pwdNueva
                                       WHERE idusuarios = @id";
            try
            {
                ParmsDictionary.Add("@pwdNueva", pwdNueva);
                ParmsDictionary.Add("@id", id);

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
