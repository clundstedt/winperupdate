using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaComponenteByName: SpDao
    {
        public SqlDataReader Execute(int idVersion, string nameFile)
        {
            SpName = @" select * from Componentes where idVersion = @idVersion and NameFile = @nameFile";
            try
            {
                ParmsDictionary.Add("@idVersion", idVersion);
                ParmsDictionary.Add("@nameFile", nameFile);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute(int idVersion, string nameFile)", ex.Message);
                throw new Exception(msg, ex);
            }
        }
        public SqlDataReader Execute(string nameFile)
        {
            SpName = @" select * from Componentes where NameFile = @nameFile";
            try
            {
                ParmsDictionary.Add("@nameFile", nameFile);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute(string nameFile)", ex.Message);
                throw new Exception(msg, ex);
            }
        }

    }
}
