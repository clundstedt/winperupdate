using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaComuna : SpDao
    {
        public SqlDataReader Execute(int idRgn)
        {
            SpName = @"select * from Cmn where idRgn = @idRgn";
            try
            {
                ParmsDictionary.Add("@idRgn", idRgn);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader Execute(int idRgn, int idCmn)
        {
            SpName = @"select * from Cmn where (idRgn = @idRgn or @idRgn = 0) And idCmn = @idCmn";
            try
            {
                ParmsDictionary.Add("@idRgn", idRgn);
                ParmsDictionary.Add("@idCmn", idCmn);

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
