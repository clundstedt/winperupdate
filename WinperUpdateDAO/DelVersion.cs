using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class DelVersion : SpDao
    {
        public int Execute(int idVersion)
        {
            SpName = @" delete Versiones where idVersion = @idVersion;";
            try
            {
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
