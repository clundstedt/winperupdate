using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaClienteByNroLicencia : SpDao
    {
        public SqlDataReader Execute(string nroLicencia)
        {
            SpName = @"select * from Clientes where NroLicencia = @nroLicencia";
            try
            {
                ParmsDictionary.Add("@nroLicencia", @nroLicencia);

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
