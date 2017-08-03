using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddTareas : SpDao
    {
        public int Execute(int idTareas, int idClientes, int idAmbientes, int CodPrf
                          ,int Estado, string Modulo, int idVersion, string NameFile
                          ,string Error)
        {
            SpName = @"INSERT INTO tareas (idClientes
                                          ,idAmbientes
                                          ,CodPrf
                                          ,Estado
                                          ,Modulo
                                          ,idVersion
                                          ,NameFile
                                          ,Error
                                          ,FechaRegistro
                                          ,Reportado)
                                    VALUES(@idClientes
                                          ,@idAmbientes
                                          ,@CodPrf
                                          ,@Estado
                                          ,@Modulo
                                          ,@idVersion
                                          ,@NameFile
                                          ,@Error
                                          ,GetDate()
                                          ,0)";
            try
            {
                ParmsDictionary.Add("@idClientes",idClientes);
                ParmsDictionary.Add("@idAmbientes",idAmbientes);
                ParmsDictionary.Add("@CodPrf",CodPrf);
                ParmsDictionary.Add("@Estado",Estado);
                ParmsDictionary.Add("@Modulo",Modulo);
                ParmsDictionary.Add("@idVersion",idVersion);
                ParmsDictionary.Add("@NameFile",NameFile);
                ParmsDictionary.Add("@Error",Error);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public bool Execute(DataTable dt)
        {
            try
            {
                object[,] obj = new object[dt.Rows.Count, 2];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[] data =
                    {
                        "@idtarea"+i,
                        "@idcliente"+i,
                        "@idambiente"+i,
                        "@codprf"+i,
                        "@estado"+i,
                        "@modulo"+i,
                        "@idversion"+i,
                        "@namefile"+i,
                        "@error"+i,
                    };
                    var parms = new ConnectorDB.ThDictionary();
                    parms.Add(data[0], dt.Rows[i][0]);
                    parms.Add(data[1], dt.Rows[i][1]);
                    parms.Add(data[2], dt.Rows[i][2]);
                    parms.Add(data[3], dt.Rows[i][3]);
                    parms.Add(data[4], dt.Rows[i][4]);
                    parms.Add(data[5], dt.Rows[i][5]);
                    parms.Add(data[6], dt.Rows[i][6]);
                    parms.Add(data[7], dt.Rows[i][7]);
                    parms.Add(data[8], dt.Rows[i][8]);
                    obj[i, 0] = string.Format(@"INSERT INTO tareas (idClientes
                                          ,idAmbientes
                                          ,CodPrf
                                          ,Estado
                                          ,Modulo
                                          ,idVersion
                                          ,NameFile
                                          ,Error
                                          ,FechaRegistro
                                          ,Reportado)
                                    VALUES({0}
                                          ,{1}
                                          ,{2}
                                          ,{3}
                                          ,{4}
                                          ,{5}
                                          ,{6}
                                          ,{7}
                                          ,GetDate()
                                          ,0)", data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8]);
                    obj[i, 1] = parms;
                }
                return Connector.ExecuteQueryTrans(obj);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
