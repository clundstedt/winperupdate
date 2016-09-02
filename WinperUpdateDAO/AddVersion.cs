using System;
using System.Data.SqlClient;

namespace WinperUpdateDAO
{
    public class AddVersion : SpDao
    {
        public int Execute(string numVersion, DateTime fecVersion, char estado)
        {
            SpName = @" insert into Versiones (NumVersion, FecVersion, Estado) values (@numVersion, @fecVersion, @estado)";
            try
            {
                ParmsDictionary.Add("@numVersion", numVersion);
                ParmsDictionary.Add("@fecVersion", fecVersion);
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
