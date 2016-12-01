using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddComponenteModulo : SpDao
    {
        public int ExecuteComponenteModulo(string Nombre, string Descripcion, int Modulos, int TipoComponentes)
        {
            SpName = @"INSERT INTO ComponentesModulos (Nombre
                                                      ,Descripcion
                                                      ,Modulos
                                                      ,TipoComponentes)
                                                VALUES(@Nombre
                                                      ,@Descripcion
                                                      ,@Modulos
                                                      ,@TipoComponentes)";
            try
            {
                ParmsDictionary.Add("@Nombre", Nombre);
                ParmsDictionary.Add("@Descripcion", Descripcion);
                ParmsDictionary.Add("@Modulos", Modulos);
                ParmsDictionary.Add("@TipoComponentes", TipoComponentes);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteComponenteModulo", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int ExecuteTipoComponentes(string Nombre, bool isCompBD)
        {
            SpName = @"INSERT INTO TipoComponentes (Nombre
                                                   ,isCompBD)
                                            VALUES (@Nombre
                                                   ,@isCompBD)";
            try
            {
                ParmsDictionary.Add("@Nombre", Nombre);
                ParmsDictionary.Add("@isCompBD", isCompBD);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteTipoComponentes", ex.Message);
                throw new Exception(msg, ex);
            }
        }

    }
}
