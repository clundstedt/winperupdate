using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaClientes : SpDao
    {
        public new SqlDataReader Execute()
        {
            SpName = @"select * from Clientes";
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

        /// <summary>
        /// Obtiene los clientes que tienen una version especificada
        /// </summary>
        /// <param name="idVersion">ID de la version</param>
        /// <returns></returns>
        public SqlDataReader Execute(int idVersion)
        {
            SpName = @"SELECT c.* 
                              FROM clientes c
                              INNER JOIN Versiones_has_Clientes vhc
                              ON c.idClientes = vhc.idClientes
                              WHERE vhc.idVersion = @idVersion";
            try
            {
                ParmsDictionary.Add("@idVersion", idVersion);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader GetFolio(string identificador)
        {
            SpName = @"SELECT MAX(numero) 
                              FROM folios 
                              WHERE identificador = @identificador";
            try
            {
                ParmsDictionary.Add("@identificador", identificador);

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
