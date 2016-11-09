using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WinperUpdateDAO
{
    public class CnaAmbientes:SpDao
    {
        /// <summary>
        /// Lista todos los ambientes de un cliente.
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public SqlDataReader Execute(int idCliente)
        {
            SpName = @" SELECT	a1.*, ' ' as Estado
                        FROM	ambientes a1
                        WHERE	a1.idClientes = @idCliente
                        ";
            try
            {
                ParmsDictionary.Add("@idCliente", idCliente);
                return Connector.ExecuteQuery(SpName,ParmsDictionary);
            }catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader Execute(int idCliente, int idVersion)
        {
            SpName = @" SELECT	a1.*, a2.Estado
                        FROM	ambientes a1
                        LEFT JOIN  Versiones_has_Clientes_has_Ambientes a2
                        on      a2.idClientes = a1.idClientes
                        and     a2.idAmbientes = a1.idAmbientes
                        and     a2.idVersion   = @idVersion
                        WHERE	a1.idClientes = @idCliente
                        ";
            try
            {
                ParmsDictionary.Add("@idVersion", idVersion);
                ParmsDictionary.Add("@idCliente", idCliente);
                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
        /// <summary>
        /// Obtiene el estado del ambiente segun las tareas programadas
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="idAmbiente"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteAmbienteOK(int idVersion, int idAmbiente)
        {
            SpName = @"SELECT dbo.fcn_ambienteok(@idVersion,@idAmbiente) AS OK";
            try
            {
                ParmsDictionary.Add("@idVersion", idVersion);
                ParmsDictionary.Add("@idAmbiente", idAmbiente);

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
