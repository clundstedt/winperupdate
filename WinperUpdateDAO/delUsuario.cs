using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class delUsuario : SpDao
    {
        public int Execute(int id)
        {
            SpName = @" update Usuarios 
                           set EstUsr = 'C'
                         where idUsuarios = @id";
            try
            {
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
