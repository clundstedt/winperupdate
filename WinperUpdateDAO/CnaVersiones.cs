using System;
using System.Data.SqlClient;

namespace WinperUpdateDAO
{
    public class CnaVersiones : SpDao
    {
        public new SqlDataReader Execute()
        {
            SpName = @" select * from Versiones";
            try
            {
                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader Execute(string release)
        {
            SpName = @" select * from Versiones where NumVersion = @release";
            try
            {
                ParmsDictionary.Add("@release", release);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
        
        public SqlDataReader Execute(int id)
        {
            try
            {
                SpName = @"SELECT * FROM versiones WHERE idVersion = @id";

                ParmsDictionary.Add("@id", id);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
