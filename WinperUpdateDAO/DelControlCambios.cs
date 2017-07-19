using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class DelControlCambios : SpDao
    {
        public int ExecuteDocCambios(string doc, int version, int tips, int modulo)
        {
            try
            {
                SpName = @"DELETE FROM DocCambios WHERE Nombre = @nombre
                                                    AND VersionCC = @version
                                                    AND TipsCC = @tips
                                                    AND ModuloCC = @modulo";
                ParmsDictionary.Add("@nombre", doc);
                ParmsDictionary.Add("@version", version);
                ParmsDictionary.Add("@tips", tips);
                ParmsDictionary.Add("@modulo", modulo);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int Execute(int version, int tips, int modulo)
        {
            try
            {
                SpName = @"DELETE FROM controlcambios WHERE version = @version AND tips = @tips AND modulo = @modulo";

                ParmsDictionary.Add("@version", version);
                ParmsDictionary.Add("@tips", tips);
                ParmsDictionary.Add("@modulo", modulo);

                return Connector.ExecuteQueryNoResult(SpName,ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
