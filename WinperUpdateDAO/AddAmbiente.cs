using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WinperUpdateDAO
{
    public class AddAmbiente : SpDao
    {
        public int Execute(int idCliente, string Nombre, int Tipo, string ServerBd
            , string Instancia, string NomBd, string UserDbo, string PwdDbo)
        {
            SpName = @"INSERT INTO ambientes (idClientes
                                            ,Nombre
                                            ,Tipo
                                            ,ServerBd
                                            ,Instancia
                                            ,NomBd
                                            ,UserDbo
                                            ,PwdDbo)
                                      VALUES(@idCliente
                                            ,@Nombre
                                            ,@Tipo
                                            ,@ServerBd
                                            ,@Instancia
                                            ,@Nombd
                                            ,@UserDbo
                                            ,@PwdDbo)";
            try
            {
                ParmsDictionary.Add("@idCliente", idCliente);
                ParmsDictionary.Add("@Nombre", Nombre);
                ParmsDictionary.Add("@Tipo", Tipo);
                ParmsDictionary.Add("@ServerBd", ServerBd);
                ParmsDictionary.Add("@Instancia", Instancia);
                ParmsDictionary.Add("@Nombd", NomBd);
                ParmsDictionary.Add("@UserDbo", UserDbo);
                ParmsDictionary.Add("@PwdDbo", PwdDbo);
                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);

            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public bool ExecuteAmbientesXLSX(System.Data.DataTable dt)
        {
            
            SpName = @"INSERT INTO ambientesxlsx 
                                                (idAmbientes
                                                ,idClientes
                                                ,Nombre
                                                ,Tipo
                                                ,ServerBd
                                                ,Instancia
                                                ,NomBd
                                                ,UserDbo
                                                ,PwdDbo
                                                ,FechaRegistro
                                                ,EstadoRegistro
                                                ,ErrorRegistro) 
                                          VALUES({0}
                                                ,{1}
                                                ,{2}
                                                ,{3}
                                                ,{4}
                                                ,{5}
                                                ,{6}
                                                ,{7}
                                                ,{8}
                                                ,{9}
                                                ,{10}
                                                ,{11})";
            object[,] querys = new object[dt.Rows.Count+1, 2];
            try
            {
                int idCliente = int.Parse(dt.Rows[0][0].ToString());
                string del = @"DELETE FROM ambientesxlsx WHERE idClientes = @idClienteDel";
                ParmsDictionary.Add("@idClienteDel", idCliente);
                querys[0, 0] = del;
                querys[0, 1] = ParmsDictionary;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //Se crean los parametros por iteración, para evitar el conflicto de repetición de nombres
                    string[] datos =
                    {
                        "@idAmbientes"+i,
                        "@idClientes" + i,
                        "@Nombre" + i,
                        "@Tipo" + i,
                        "@ServerBd" + i,
                        "@Instancia" + i,
                        "@NomBd" + i,
                        "@UserDbo" + i,
                        "@PwdDbo" + i,
                        "@FechaRegistro" + i,
                        "@EstadoRegistro" + i,
                        "@ErrorRegistro" + i
                    };
                    //Se da formato a la sentencia, asignandoles los parametros creados antes
                    var sql = string.Format(SpName
                        , datos[0], datos[1], datos[2], datos[3], datos[4], datos[5], datos[6], datos[7]
                        , datos[8], datos[9], datos[10], datos[11]);
                    //Se crea el objeto ThDictionary, para entregar los parametros formateados antes
                    var parms = new ConnectorDB.ThDictionary();
                    parms.Add(datos[0], i+1);
                    parms.Add(datos[1], idCliente);
                    parms.Add(datos[2], dt.Rows[i][1].ToString());
                    parms.Add(datos[3], dt.Rows[i][2].ToString());
                    parms.Add(datos[4], dt.Rows[i][3].ToString());
                    parms.Add(datos[5], dt.Rows[i][4].ToString());
                    parms.Add(datos[6], dt.Rows[i][5].ToString());
                    parms.Add(datos[7], dt.Rows[i][6].ToString());
                    parms.Add(datos[8], dt.Rows[i][7].ToString());
                    parms.Add(datos[9], dt.Rows[i][8].ToString());
                    parms.Add(datos[10], dt.Rows[i][9].ToString());
                    parms.Add(datos[11], dt.Rows[i][10].ToString());
                    //Se asigna a la matriz, cada valor, la sentencia formateada y el objeto que contiene los parametros
                    querys[i+1, 0] = sql;
                    querys[i+1, 1] = parms;
                }
                return Connector.ExecuteQueryTrans(querys);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteAmbientesXLSX", ex.Message);
                throw new Exception(msg, ex);
            }
        }
        /// <summary>
        /// Inserta los AmbientesXLSX en Ambientes 
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public int ExecuteAmbXLSXtoAmb(int idCliente)
        {
            SpName = @"INSERT INTO Ambientes (idClientes
                                            ,Nombre
					                        ,Tipo
					                        ,ServerBd
					                        ,Instancia
					                        ,NomBd
					                        ,UserDbo
					                        ,PwdDbo)
			                        SELECT  idClientes
				                            ,Nombre
					                        ,Tipo
					                        ,ServerBd
					                        ,Instancia
					                        ,NomBd
					                        ,UserDbo
					                        ,PwdDbo
			                        FROM AmbientesXLSX
			                        WHERE idClientes = @idCliente";
            try
            {
                ParmsDictionary.Add("@idCliente", idCliente);
                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteAmbXLSXtoAmb", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
