using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class UpdCliente : SpDao
    {
        public int Execute(int id, int rut, char dv, string nombre, string direccion, int idCmn
                          ,string nrolicencia, int estmtc, string mesini, string nrotrbc
                          ,string nrotrbh, string nrousr, string mescon, int corr)
        {
            SpName = @" update Clientes 
                           set Rut = @rut, 
                               Dv = @dv, 
                               RazonSocial = @nombre, 
                               Direccion = @direccion, 
                               idCmn = @idCmn,
                               NroLicencia = @nrolicencia,
                               EstMtc = @estmtc,
                               MesIni = @mesini, 
                               NroTrbc = @nrotrbc, 
                               NroTrbh = @nrotrbh, 
                               NroUsr = @nrousr,
                               MesCon = @mescon,
                               Correlativo = @corr 
                         where idClientes = @id";
            try
            {
                ParmsDictionary.Add("@rut", rut);
                ParmsDictionary.Add("@dv", dv);
                ParmsDictionary.Add("@nombre", nombre);
                ParmsDictionary.Add("@direccion", direccion);
                ParmsDictionary.Add("@idCmn", idCmn);
                ParmsDictionary.Add("@id", id);
                ParmsDictionary.Add("@nrolicencia", nrolicencia);
                ParmsDictionary.Add("@estmtc", estmtc);
                ParmsDictionary.Add("@mesini", mesini);
                ParmsDictionary.Add("@nrotrbc", nrotrbc);
                ParmsDictionary.Add("@nrotrbh", nrotrbh);
                ParmsDictionary.Add("@nrousr", nrousr);
                ParmsDictionary.Add("@mescon", mescon);
                ParmsDictionary.Add("@corr", corr);

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
