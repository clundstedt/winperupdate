using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class PubVersion : SpDao
    {
        public int Execute(int idVersion, char estado)
        {
            SpName = @" update Versiones 
                           set Estado = @estado
                         where idVersion = @idVersion";
            try
            {
                ParmsDictionary.Add("@estado", estado);
                ParmsDictionary.Add("@idVersion", idVersion);

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
