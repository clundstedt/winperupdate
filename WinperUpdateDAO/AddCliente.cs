using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WinperUpdateDAO
{
    public class AddCliente : SpDao
    {
        public System.Data.SqlClient.SqlDataReader Execute(int rut, char dv, string nombre, string direccion, int idCmn
            , string nrolicencia, int numfolio, int estmtc, string mesini, string nrotrbc, string nrotrbh, string nrousr)
        {
            SpName = @"EXEC sp_ins_cliente @idCmn, @rut, @dv, @nombre, @direccion
                                          ,@nrolicencia, @numfolio, @estmtc, @mesini
                                          ,@nrotrbc, @nrotrbh, @nrousr";
            try
            {
                ParmsDictionary.Add("@rut", rut);
                ParmsDictionary.Add("@dv", dv);
                ParmsDictionary.Add("@nombre", nombre);
                ParmsDictionary.Add("@direccion", direccion);
                ParmsDictionary.Add("@idCmn", idCmn);
                ParmsDictionary.Add("@nrolicencia", nrolicencia);
                ParmsDictionary.Add("@numfolio", numfolio);
                ParmsDictionary.Add("@estmtc", estmtc);
                ParmsDictionary.Add("@mesini", mesini);
                ParmsDictionary.Add("@nrotrbc", nrotrbc);
                ParmsDictionary.Add("@nrotrbh", nrotrbh);
                ParmsDictionary.Add("@nrousr", nrousr);

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
