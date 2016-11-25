using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddVersionToAmbiente : SpDao
    {
        public int Execute(int idCliente, int idVersion, int idAmbiente, char estado)
        {
            SpName = @"INSERT INTO Versiones_has_Clientes_has_Ambientes (
                                             idClientes
                                            ,idAmbientes
                                            ,idVersion
                                            ,Estado
                                            ,FechaInstalacion)
                                      VALUES(@idCliente
                                            ,@idAmbiente
                                            ,@idVersion
                                            ,@Estado
                                            ,@FechaInstalacion)";
            try
            {
                ParmsDictionary.Add("@idCliente", idCliente);
                ParmsDictionary.Add("@idAmbiente", idAmbiente);
                ParmsDictionary.Add("@idVersion", idVersion);
                ParmsDictionary.Add("@Estado", estado);
                ParmsDictionary.Add("@FechaInstalacion", DateTime.Now);

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
