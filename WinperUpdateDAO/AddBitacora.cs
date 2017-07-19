using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddBitacora :SpDao
    {
        public int Execute(string menu, string vant, string vnue, char accion, DateTime fecha, int usuario, string registro)
        {
            try
            {
                SpName = @"INSERT INTO bitacora (Menu
                                                , Vant
                                                , Vnue
                                                , Accion
                                                , Fecha
                                                , Usuario
                                                , Registro)
                                         VALUES (@menu
                                                , @vant
                                                , @vnue
                                                , @accion
                                                , @fecha
                                                , @usuario
                                                , @registro)";
                
                ParmsDictionary.Add("@menu",menu);
                ParmsDictionary.Add("@vant",vant);
                ParmsDictionary.Add("@vnue",vnue);
                ParmsDictionary.Add("@accion",accion);
                ParmsDictionary.Add("@fecha",fecha);
                ParmsDictionary.Add("@usuario",usuario);
                ParmsDictionary.Add("@registro", registro);

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
                SpName = @"INSERT INTO bitacora (Menu
                                                , Vant
                                                , Vnue
                                                , Accion
                                                , Fecha
                                                , Usuario
                                                , Registro)
                                         VALUES ({0}
                                                , {1}
                                                , {2}
                                                , {3}
                                                , {4}
                                                , {5}
                                                , {6})";
                object[,] obj = new object[dt.Rows.Count, 2];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[] data =
                    {
                        "@menu"+i,
                        "@vant"+i,
                        "@vnue"+i,
                        "@accion"+i,
                        "@fecha"+i,
                        "@usuario"+i,
                        "@Registro"+i,
                    };

                    var parms = new ConnectorDB.ThDictionary();
                    parms.Add(data[0], dt.Rows[i][0]);
                    parms.Add(data[1], dt.Rows[i][1]);
                    parms.Add(data[2], dt.Rows[i][2]);
                    parms.Add(data[3], dt.Rows[i][3]);
                    parms.Add(data[4], dt.Rows[i][4]);
                    parms.Add(data[5], dt.Rows[i][5]);
                    parms.Add(data[6], dt.Rows[i][6]);

                    obj[i, 0] = string.Format(SpName, data[0], data[1], data[2], data[3], data[4], data[5], data[6]);
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
