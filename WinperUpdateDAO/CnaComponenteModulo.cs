using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaComponenteModulo : SpDao
    {
        public SqlDataReader ExecuteComponentesModulos()
        {
            SpName = @"SELECT cm.*, tc.Nombre as NombreTipo, tc.isCompBD, tc.isCompDLL, tc.Extensiones, tc.isCompCambios
                                                      FROM ComponentesModulos cm
                                                INNER JOIN TipoComponentes tc
                                                        ON cm.TipoComponentes = tc.idTipoComponentes";
            try
            {
                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteComponentesModulos", ex.Message);
                throw new Exception(msg, ex);
            }
        }
        public SqlDataReader ExecuteComponentesModulos(int idModulo)
        {
            SpName = @"SELECT cm.*, tc.Nombre as NombreTipo, tc.isCompBD, tc.isCompDLL, tc.Extensiones, tc.isCompCambios
                                                      FROM ComponentesModulos cm
                                                INNER JOIN TipoComponentes tc
                                                        ON cm.TipoComponentes = tc.idTipoComponentes
                                                     WHERE cm.Modulos = @idModulo";
            try
            {
                ParmsDictionary.Add("@idModulo", idModulo);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteComponentesModulos", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader ExecuteComponentesModulos(string nombreComponente)
        {
            SpName = @"SELECT cm.*, m.NomModulo FROM ComponentesModulos cm INNER JOIN Modulos m ON cm.Modulos = m.idModulo WHERE cm.Nombre = @Nombre";
            try
            {
                ParmsDictionary.Add("@Nombre", nombreComponente);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteComponentesModulos", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader ExecuteTipoComponente()
        {
            SpName = @"SELECT * FROM TipoComponentes";
            try
            {
                return Connector.ExecuteQuery(SpName, null);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader ExecuteConDirectorio()
        {
            SpName = @"SELECT cm.*, m.directorio, m.NomModulo FROM ComponentesModulos cm INNER JOIN Modulos m
                                                                ON cm.Modulos = m.idModulo
                                                             WHERE m.isCore = 1
                                                               AND m.Estado = 'V'";
            try
            {
                return Connector.ExecuteQuery(SpName, null);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteConDirectorio", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader ExecuteConDirectorio(int idComp)
        {
            SpName = @"SELECT cm.*, m.directorio, m.NomModulo
                                            FROM ComponentesModulos cm INNER JOIN Modulos m
                                              ON cm.Modulos = m.idModulo
                                           WHERE cm.idComponentesModulos = @id";
            try
            {
                ParmsDictionary.Add("@id", idComp);
                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteConDirectorio", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public SqlDataReader ExecuteConDirectorioByModulo(int idModulo)
        {
            SpName = @"SELECT cm.*, m.directorio, m.NomModulo
                                            FROM ComponentesModulos cm INNER JOIN Modulos m
                                              ON cm.Modulos = m.idModulo
                                           WHERE cm.Modulos = @id";
            try
            {
                ParmsDictionary.Add("@id", idModulo);
                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteConDirectorio", ex.Message);
                throw new Exception(msg, ex);
            }
        }
        public SqlDataReader ExecuteTipoComponentesByVersion(int idVersion)
        {
            SpName = @"SELECT DISTINCT tc.* 
                                       FROM TipoComponentes tc LEFT JOIN ComponentesModulos cm
                                         ON tc.idTipoComponentes = cm.TipoComponentes
                                      WHERE cm.Nombre IN (SELECT NameFile FROM Componentes WHERE idVersion = @idVersion)
                                         OR tc.isCompBD = 1";
            try
            {
                ParmsDictionary.Add("@idVersion", idVersion);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteByVersion", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
