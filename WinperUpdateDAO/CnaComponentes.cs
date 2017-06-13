using System;
using System.Data.SqlClient;

namespace WinperUpdateDAO
{
    public class CnaComponentes : SpDao
    {
        public SqlDataReader Execute(int idVersion)
        {
            SpName = @" select * from Componentes where idVersion = @idVersion";
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

        public SqlDataReader Execute(int idVersion, string idModulo)
        {
            SpName = @" select * from Componentes where idVersion = @idVersion and Modulo = @idModulo";
            try
            {
                ParmsDictionary.Add("@idVersion", idVersion);
                ParmsDictionary.Add("@idModulo", idModulo);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader ExecuteConDirectorio(int idVersion)
        {
            try
            {
                SpName = @"select c.*, m.Directorio 
                             from Componentes c, Modulos m 
                            where c.idVersion = @idVersion 
                              and c.Modulo = m.NomModulo";
                ParmsDictionary.Add("@idVersion", idVersion);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "{NombreFuncion}", ex.Message);
                throw new Exception(msg, ex);
            }
        }

    }
}
