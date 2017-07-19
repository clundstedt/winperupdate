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
        public System.Data.SqlClient.SqlDataReader ExecuteComponenteModulo(string xml)
        {
            try
            {
                SpName = @"EXEC sp_AddComponentesModulos @xml";

                ParmsDictionary.Add("@xml", xml);
                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteComponenteModulo", ex.Message);
                throw new Exception(msg, ex);
            }
        }
        public int ExecuteTipoComponentes(string Nombre, bool isCompBD, bool isCompDLL, string Extensiones, bool isCompCambios)
        {
            SpName = @"INSERT INTO TipoComponentes (Nombre
                                                   ,isCompBD
                                                   ,isCompDLL
                                                   ,Extensiones
                                                   ,isCompCambios)
                                            VALUES (@Nombre
                                                   ,@isCompBD
                                                   ,@isCompDLL
                                                   ,@Extensiones
                                                   ,@isCompCambios)";
            try
            {
                ParmsDictionary.Add("@Nombre", Nombre);
                ParmsDictionary.Add("@isCompBD", isCompBD);
                ParmsDictionary.Add("@isCompDLL", isCompDLL);
                ParmsDictionary.Add("@Extensiones", Extensiones);
                ParmsDictionary.Add("@isCompCambios", isCompCambios);

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
