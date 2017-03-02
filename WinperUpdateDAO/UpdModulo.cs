using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class UpdModulo : SpDao
    {
        public int Execute(int idModulo, string NomModulo, string Descripcion, bool isCore, string Directorio, int Suite)
        {
            SpName = @"UPDATE modulos SET NomModulo = @NomModulo
                                         ,Descripcion = @Descripcion
                                         ,isCore = @isCore
                                         ,Directorio = @Directorio
                                         ,Suite = @Suite
                                    WHERE idModulo = @idModulo";
            try
            {
                ParmsDictionary.Add("@NomModulo", NomModulo);
                ParmsDictionary.Add("@Descripcion", Descripcion);
                ParmsDictionary.Add("@isCore", isCore);
                ParmsDictionary.Add("@Directorio", Directorio);
                ParmsDictionary.Add("@idModulo", idModulo);
                ParmsDictionary.Add("@Suite", Suite);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int ExecuteVigente(int idModulo)
        {
            SpName = @"UPDATE modulos SET estado = 'V' WHERE idModulo = @idModulo";
            try
            {
                ParmsDictionary.Add("@idModulo", idModulo);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteVigente", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
