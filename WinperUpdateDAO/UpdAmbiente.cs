using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WinperUpdateDAO
{
    public class UpdAmbiente : SpDao
    {
        public int Execute(int idAmbiente, int idCliente, string Nombre, int Tipo, string ServerBd
            ,string Instancia, string NomBd, string UserDbo, string PwdDbo)
        {
            SpName = @"UPDATE ambientes SET 
                                            Nombre = @Nombre,
                                            Tipo = @Tipo,
                                            ServerBd = @ServerBd,
                                            Instancia = @Instancia,
                                            NomBd = @NomBd,
                                            UserDbo = @UserDbo,
                                            PwdDbo = @PwdDbo
                                        WHERE 
                                            idAmbientes = @idAmbiente AND idClientes = @idCliente";
            try
            {
                ParmsDictionary.Add("@Nombre", Nombre);
                ParmsDictionary.Add("@Tipo",Tipo);
                ParmsDictionary.Add("@ServerBd",ServerBd);
                ParmsDictionary.Add("@Instancia",Instancia);
                ParmsDictionary.Add("@NomBd",NomBd);
                ParmsDictionary.Add("@UserDbo",UserDbo);
                ParmsDictionary.Add("@PwdDbo",PwdDbo);
                ParmsDictionary.Add("@idAmbiente",idAmbiente);
                ParmsDictionary.Add("@idCliente",idCliente);
                return Connector.ExecuteQueryNoResult(SpName,ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
