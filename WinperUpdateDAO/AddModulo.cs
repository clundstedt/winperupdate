using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddModulo : SpDao
    {
        public int Execute(string NomModulo, string Descripcion, bool iscore, string Directorio, int Suite)
        {
            SpName = @"INSERT INTO modulos (NomModulo
                                           ,Descripcion
                                           ,isCore
                                           ,Directorio
                                           ,Suite)
                                    output INSERTED.idModulo
                                    VALUES (@NomModulo
                                           ,@Descripcion
                                           ,@isCore
                                           ,@Directorio
                                           ,@Suite)";
            try
            {
                ParmsDictionary.Add("@NomModulo", NomModulo);
                ParmsDictionary.Add("@Descripcion", Descripcion);
                ParmsDictionary.Add("@isCore", iscore);
                ParmsDictionary.Add("@Directorio", Directorio);
                ParmsDictionary.Add("@Suite", Suite);

                return (int)Connector.ExecuteQueryScalar(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public bool ExecuteTrans(System.Data.DataTable dt)
        {
            SpName = @"INSERT INTO modulosxlsx (NomModulo
                                               ,Descripcion
                                               ,isCore
                                               ,Directorio
                                               ,FechaRegistro
                                               ,EstadoRegistro
                                               ,ErrorRegistro
                                               ,Usuario
                                               ,Suite)
                                        VALUES ({0}
                                               ,{1}
                                               ,{2}
                                               ,{3}
                                               ,{4}
                                               ,{5}
                                               ,{6}
                                               ,{7}
                                               ,{8})";
            string sqlDel = @"DELETE FROM modulosxlsx WHERE Usuario = @idUsuarioDel";
            try
            {
                int idUsuario = int.Parse(dt.Rows[0][6].ToString());
                object[,] obj = new object[dt.Rows.Count+1, 2];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[] data =
                    {
                        "@NomModulo"+i,
                        "@Descripcion"+i,
                        "@isCore"+i,
                        "@Directorio"+i,
                        "@FechaRegistro"+i,
                        "@EstadoRegistro"+i,
                        "@ErrorRegistro"+i,
                        "@idUsuario"+i,
                        "@Suite"+i
                    };
                    var parm = new ConnectorDB.ThDictionary();
                    parm.Add(data[0], dt.Rows[i][0]);
                    parm.Add(data[1], dt.Rows[i][1]);
                    parm.Add(data[2], dt.Rows[i][2]);
                    parm.Add(data[3], dt.Rows[i][3]);
                    parm.Add(data[4], DateTime.Now);
                    parm.Add(data[5], dt.Rows[i][4]);
                    parm.Add(data[6], dt.Rows[i][5]);
                    parm.Add(data[7], idUsuario);
                    parm.Add(data[8], dt.Rows[i][7]);
                    obj[i+1, 0] = string.Format(SpName, data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8]);
                    obj[i+1, 1] = parm;
                }
                ParmsDictionary.Add("@idUsuarioDel", idUsuario);
                obj[0, 0] = string.Format(sqlDel, idUsuario);
                obj[0, 1] = ParmsDictionary;
                return Connector.ExecuteQueryTrans(obj);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteTrans", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int ExecuteFromXLSX(int idUsuario)
        {
            SpName = @"INSERT INTO modulos (NomModulo
                                            ,Descripcion
                                            ,isCore
                                            ,Directorio
                                            ,Estado
                                            ,Suite) 
                                    SELECT NomModulo
                                            ,Descripcion
                                            ,isCore
                                            ,Directorio
                                            ,'V'
                                            ,Suite
                                      FROM ModulosXLSX
                                     WHERE Usuario = @idUsuario";
            try
            {
                ParmsDictionary.Add("@idUsuario",idUsuario);
                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteFromXLSX", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
