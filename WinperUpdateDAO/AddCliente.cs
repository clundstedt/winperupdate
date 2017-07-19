using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WinperUpdateDAO
{
    public class AddCliente : SpDao
    {
        public object Execute(int rut, char dv, string nombre, string direccion, int idCmn
            , string nrolicencia, int numfolio, int estmtc, string mesini, string nrotrbc
            , string nrotrbh, string nrousr, string mescon, int corr, char est)
        {
            SpName = @"INSERT INTO clientes(idCmn
						,Rut
						,Dv
						,RazonSocial
						,Direccion
						,NroLicencia
						,Folio
						,EstMtc
						,MesIni
						,NroTrbc
						,NroTrbh
						,NroUsr
                        ,MesCon
                        ,Correlativo
                        ,Estado)
                output INSERTED.idClientes
				VALUES(@idCmn
					  ,@Rut
					  ,@Dv
					  ,@RazonSocial
					  ,@Direccion
					  ,@NroLicencia
					  ,@NumFolio
					  ,@EstMtc
					  ,@MesIni
					  ,@NroTrbc
					  ,@NroTrbh
					  ,@NroUsr
                      ,@MesCon
                      ,@Corr
                      ,@Est)";
            try
            {
                ParmsDictionary.Add("@Rut", rut);
                ParmsDictionary.Add("@Dv", dv);
                ParmsDictionary.Add("@RazonSocial", nombre);
                ParmsDictionary.Add("@Direccion", direccion);
                ParmsDictionary.Add("@idCmn", idCmn);
                ParmsDictionary.Add("@NroLicencia", nrolicencia);
                ParmsDictionary.Add("@NumFolio", numfolio);
                ParmsDictionary.Add("@EstMtc", estmtc);
                ParmsDictionary.Add("@MesIni", mesini);
                ParmsDictionary.Add("@NroTrbc", nrotrbc);
                ParmsDictionary.Add("@NroTrbh", nrotrbh);
                ParmsDictionary.Add("@NroUsr", nrousr);
                ParmsDictionary.Add("@MesCon", mescon);
                ParmsDictionary.Add("@Corr", corr);
                ParmsDictionary.Add("@Est", est);

                return Connector.ExecuteQueryScalar(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public bool ExecuteClientesHasModulos(int idCliente, int[] idModulos)
        {
            SpName = @"INSERT INTO Clientes_has_Modulos (idClientes
                                                              ,idModulo)
                                                        VALUES({0}
                                                              ,{1})";
            string del = @"DELETE FROM Clientes_has_Modulos 
                                                            WHERE idClientes = @idClienteDel";
            object[,] querys = new object[idModulos.Length+1, 2];
            try
            {
                for (int i = 0; i < idModulos.Length; i++)
                {
                    string[] datos =
                    {
                        "@idCliente"+i,
                        "@idModulo"+i
                    };
                    var sql = string.Format(SpName,datos[0],datos[1]);
                    var parms = new ConnectorDB.ThDictionary();
                    parms.Add(datos[0],idCliente);
                    parms.Add(datos[1],idModulos[i]);

                    querys[i + 1, 0] = sql;
                    querys[i + 1, 1] = parms;
                }
                ParmsDictionary.Add("@idClienteDel", idCliente);
                querys[0, 0] = del;
                querys[0, 1] = ParmsDictionary;
                return Connector.ExecuteQueryTrans(querys);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int ExecutNoVigencia(DateTime fecha, string motivo, int cliente, int usuario)
        {
            try
            {
                SpName = @"INSERT INTO Clientes_NoVigencia (Fecha, Motivo, Cliente, Usuario) VALUES(@fecha, @motivo, @cliente, @usuario)";

                ParmsDictionary.Add("@fecha", fecha);
                ParmsDictionary.Add("@motivo", motivo);
                ParmsDictionary.Add("@cliente", cliente);
                ParmsDictionary.Add("@usuario", usuario);

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
