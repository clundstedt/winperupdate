using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddComponente : SpDao
    {
        public int Execute(int idVersion, string modulo, string nameFile, string numVersion, DateTime fecVersion, char tipo, string motor)
        {
            SpName = @" insert into Componentes (idVersion, Modulo, NameFile, VersionFile, FechaFile, Comentario, Tipo, MotorSql) 
                                    values (@idVersion, @modulo, @nameFile, @numVersion, @fecVersion, '', @tipo, @motor)";
            try
            {
                ParmsDictionary.Add("@idVersion", idVersion);
                ParmsDictionary.Add("@modulo", modulo);
                ParmsDictionary.Add("@nameFile", nameFile);
                ParmsDictionary.Add("@numVersion", numVersion);
                ParmsDictionary.Add("@fecVersion", fecVersion);
                ParmsDictionary.Add("@tipo", tipo);
                ParmsDictionary.Add("@motor", motor);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public System.Data.SqlClient.SqlDataReader Execute(string xml)
        {
            SpName = @"EXEC sp_AddComponentesByXml @xml";
            try
            {
                ParmsDictionary.Add("@xml", xml);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute(string xml)", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
