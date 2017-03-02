using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class DelComponenteModulo : SpDao
    {
        public int ExecuteComponenteModulo(int idComponentesModulos)
        {
            SpName = @"DELETE FROM ComponentesModulos WHERE idComponentesModulos = @idComponentesModulos";
            try
            {
                ParmsDictionary.Add("@idComponentesModulos", idComponentesModulos);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteComponenteModulo", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int ExecuteTipoComponentes(int idTipoComponentes)
        {
            SpName = @"DELETE FROM TipoComponentes WHERE idTipoComponentes = @idTipoComponentes";
            try
            {
                ParmsDictionary.Add("@idTipoComponentes", idTipoComponentes);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteTipoComponentes", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int ExecuteAll()
        {
            try
            {
                SpName = @"DELETE FROM ComponentesModulos";

                return Connector.ExecuteQueryNoResult(SpName, null);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteAll", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
