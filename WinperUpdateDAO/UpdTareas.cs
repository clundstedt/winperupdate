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
    }
}
