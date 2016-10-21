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
    }
}
