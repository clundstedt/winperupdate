using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaSuites : SpDao
    { 
        public new SqlDataReader Execute()
        {
            try
            {
                SpName = @"SELECT * FROM Suites";

                return Connector.ExecuteQuery(SpName, null);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
