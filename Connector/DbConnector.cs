using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ConnectorDB
{
    public class DbConnector
    {
        /// <summary>
        /// string de conexión a la base de datos.
        /// </summary>
        private string ConnectionStr { get; set; }

        /// <summary>
        /// Log4Net para almacenar errores e información de depuración de la aplicación.
        /// </summary>

        public DbConnector()
        {
        }

        /// <summary>
        /// Método que realiza la obtención del connection string desde el archivo de configuración.
        /// </summary>
        private void ReadParameters()
        {
            string msg = "String de conexion vacío, revise: ConfigurationManager.ConnectionStrings['ApplicationServices'].ConnectionString";
            try
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

                if (!String.IsNullOrEmpty(ConnectionStr)) return;
                throw new NullReferenceException(msg);
            }
            catch (Exception ex)
            {
                throw new NullReferenceException(msg, ex);
            }
        }

        /// <summary>
        /// Método para ejecutar procedimientos almacenados que retornan datos.
        /// </summary>
        /// <param name="commandName">Nombre del Store Procedure a Ejecutar</param>
        /// <param name="parmsDictionary">Lista de parametros del Store Procedure</param>
        /// <returns></returns>
        public SqlDataReader ExecuteProcedure(string commandName, ThDictionary parmsDictionary)
        {
            try
            {
                ReadParameters();
            }
            catch (Exception ex)
            {
                var msg = "Error al leer connection string. " + ex.Message + ".";
                throw new Exception(msg, ex);
            }

            SqlConnection conn;
            try
            {
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception ex)
            {
                var msg = "Error al abrir la conexion" + ex.Message + ".";
                throw new Exception(msg, ex);
            }

            var comm = conn.CreateCommand();
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = commandName;
            if (parmsDictionary != null)
            {
                var iterator = parmsDictionary.GetValues();
                foreach (var kvp in iterator)
                {
                    try
                    {
                        comm.Parameters.Add(kvp.Value == null
                                                ? new SqlParameter(kvp.Key, DBNull.Value)
                                                : new SqlParameter(kvp.Key, kvp.Value));
                    }
                    catch (Exception ex)
                    {
                        var msg = "Error al agregar parámetros: " + kvp.Key + "=" + kvp.Value + " | " + ex.Message;
                        throw new Exception(msg, ex);
                    }
                }
            }
            try
            {
                return comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                var msg = "Error al ejecutar comando: " + comm + " -> " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        /// <summary>
        /// Ejecuta un query directa
        /// </summary>
        /// <param name="query">el comando sql</param>
        /// <param name="parmsDictionary">Lista de parametros de la Query</param>
        /// <returns></returns>
        public SqlDataReader ExecuteQuery(string query, ThDictionary parmsDictionary)
        {
            try
            {
                ReadParameters();
            }
            catch (Exception ex)
            {
                var msg = "Error al leer connection string. " + ex.Message + ".";
                throw new Exception(msg, ex);
            }

            SqlConnection conn;
            try
            {
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception ex)
            {
                var msg = "Error al abrir la conexion" + ex.Message + ".";
                throw new Exception(msg, ex);
            }

            var comm = conn.CreateCommand();
            comm.CommandType = CommandType.Text;
            comm.CommandText = query;
            if (parmsDictionary != null)
            {
                var iterator = parmsDictionary.GetValues();
                foreach (var kvp in iterator)
                {
                    try
                    {
                        comm.Parameters.Add(kvp.Value == null
                                                ? new SqlParameter(kvp.Key, DBNull.Value)
                                                : new SqlParameter(kvp.Key, kvp.Value));
                    }
                    catch (Exception ex)
                    {
                        var msg = "Error al agregar parámetros: " + kvp.Key + "=" + kvp.Value + " | " + ex.Message;
                        throw new Exception(msg, ex);
                    }
                }
            }
            try
            {
                return comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                var msg = "Error al ejecutar comando: " + comm + " -> " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        /// <summary>
        /// Método para ejecutar procedimientos almacenados que no retornan datos (Insert / Update / Delete ).
        /// </summary>
        /// <param name="commandName">Nombre del Store Procedure a Ejecutar</param>
        /// <param name="parmsDictionary">Lista de parametros del Store Procedure</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandName, ThDictionary parmsDictionary)
        {
            try
            {
                ReadParameters();
            }
            catch (Exception ex)
            {
                var msg = "Error al leer connection string. " + ex.Message + ".";
                throw new Exception(msg, ex);
            }

            SqlConnection conn;
            try
            {
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception ex)
            {
                var msg = "Error al abrir la conexion, " + ex.Message + ".";
                throw new Exception(msg, ex);
            }

            var comm = conn.CreateCommand();
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = commandName;
            if (parmsDictionary != null)
            {
                var iterator = parmsDictionary.GetValues();
                foreach (var kvp in iterator)
                {
                    try
                    {
                        comm.Parameters.Add(kvp.Value == null
                                                ? new SqlParameter(kvp.Key, DBNull.Value)
                                                : new SqlParameter(kvp.Key, kvp.Value));
                    }
                    catch (Exception ex)
                    {
                        var msg = "Error al agregar parámetros: " + kvp.Key + "=" + kvp.Value + " | " + ex.Message;
                        // Todo Implementar tipo correcto de excepción.
                        throw new Exception(msg, ex);
                    }
                }
            }
            try
            {
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var msg = "Error al ejecutar comando: " + comm + " -> " + ex.Message;
                // Todo Implementar tipo correcto de excepción.
                throw new Exception(msg, ex);
            }

        }

        /// <summary>
        /// Método para ejecutar querys que no retornan datos (Insert / Update / Delete ).
        /// </summary>
        /// <param name="query">Query a Ejecutar</param>
        /// <param name="parmsDictionary">Lista de parametros de la query</param>
        /// <returns></returns>
        public int ExecuteQueryNoResult(string query, ThDictionary parmsDictionary)
        {
            try
            {
                ReadParameters();
            }
            catch (Exception ex)
            {
                var msg = "Error al leer connection string. " + ex.Message + ".";
                throw new Exception(msg, ex);
            }

            SqlConnection conn;
            try
            {
                conn = new SqlConnection(ConnectionStr);
                conn.Open();
            }
            catch (Exception ex)
            {
                var msg = "Error al abrir la conexion, " + ex.Message + ".";
                throw new Exception(msg, ex);
            }

            var comm = conn.CreateCommand();
            comm.CommandType = CommandType.Text;
            comm.CommandText = query;
            if (parmsDictionary != null)
            {
                var iterator = parmsDictionary.GetValues();
                foreach (var kvp in iterator)
                {
                    try
                    {
                        comm.Parameters.Add(kvp.Value == null
                                                ? new SqlParameter(kvp.Key, DBNull.Value)
                                                : new SqlParameter(kvp.Key, kvp.Value));
                    }
                    catch (Exception ex)
                    {
                        var msg = "Error al agregar parámetros: " + kvp.Key + "=" + kvp.Value + " | " + ex.Message;
                        // Todo Implementar tipo correcto de excepción.
                        throw new Exception(msg, ex);
                    }
                }
            }
            try
            {
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var msg = "Error al ejecutar comando: " + comm + " -> " + ex.Message;
                // Todo Implementar tipo correcto de excepción.
                throw new Exception(msg, ex);
            }

        }
    }
}
