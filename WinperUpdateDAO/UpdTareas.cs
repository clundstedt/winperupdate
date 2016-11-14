using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class UpdTareas : SpDao
    {
        public int Execute(int idTareas)
        {
            SpName = @"UPDATE tareas SET Estado = 1
                                     WHERE idTareas = @idTareas";
            try
            {
                ParmsDictionary.Add("@idTareas", idTareas);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int Execute(int idTareas, int estado, string msgErr)
        {
            SpName = @"UPDATE tareas SET Estado = @estado, Error = @msgErr
                                     WHERE idTareas = @idTareas";
            try
            {
                ParmsDictionary.Add("@estado", estado);
                ParmsDictionary.Add("@msgErr", msgErr);
                ParmsDictionary.Add("@idTareas", idTareas);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int ExecuteTareaReportada(int idTarea)
        {
            SpName = @"UPDATE tareas SET reportado = 1 WHERE idtareas = @idTareas";
            try
            {
                ParmsDictionary.Add("@idTareas", idTarea);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int ExecuteTodasTareas(int idCliente, int idVersion)
        {
            SpName = @"UPDATE tareas SET reportado = 1 
                                                 WHERE idClientes = @idClientes 
                                                   AND idVersion = @idVersion
                                                   AND Estado = 0
                                                    OR Estado = 2
                                                    OR Estado = 4";
            try
            {
                ParmsDictionary.Add("@idClientes", idCliente);
                ParmsDictionary.Add("@idVersion", idVersion);

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
