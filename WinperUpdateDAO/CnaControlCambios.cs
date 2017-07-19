using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaControlCambios : SpDao
    {
        public SqlDataReader ExecuteByVersion(int idVersion)
        {
            try
            {
                SpName = @"SELECT * FROM ControlCambios WHERE version = @version";

                ParmsDictionary.Add("@version", idVersion);


                return Connector.ExecuteQuery(SpName,ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader ExecuteByModulo(int idModulo)
        {
            try
            {
                SpName = @"SELECT * FROM ControlCambios WHERE Modulo = @modulo";
                ParmsDictionary.Add("@modulo", idModulo);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader ExecuteDocCambiosByVersionAndTips(int idVersion, int tips)
        {
            try
            {
                SpName = @"SELECT * FROM DocCambios WHERE VersionCC = @id AND TipsCC = @tips";

                ParmsDictionary.Add("@id", idVersion);
                ParmsDictionary.Add("@tips", tips);

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
