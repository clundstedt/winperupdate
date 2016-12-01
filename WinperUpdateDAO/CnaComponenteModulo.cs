using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaComponenteModulo : SpDao
    {
        public SqlDataReader ExecuteComponentesModulos(int idModulo)
        {
            SpName = @"SELECT cm.*, tc.Nombre as NombreTipo, tc.isCompBD
                                                      FROM ComponentesModulos cm
                                                INNER JOIN TipoComponentes tc
                                                        ON cm.TipoComponentes = tc.idTipoComponentes
                                                     WHERE cm.Modulos = @idModulo";
            try
            {
                ParmsDictionary.Add("@idModulo", idModulo);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteComponentesModulos", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader ExecuteTipoComponente()
        {
            SpName = @"SELECT * FROM TipoComponentes";
            try
            {
                return Connector.ExecuteQuery(SpName, null);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
