using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class UpdCliente : SpDao
    {
        public int Execute(int id, int rut, char dv, string nombre, string direccion, int idCmn)
        {
            SpName = @" update Clientes 
                           set Rut = @rut, 
                               Dv = @dv, 
                               RazonSocial = @nombre, 
                               Direccion = @direccion, 
                               idCmn = @idCmn
                         where idClientes = @id";
            try
            {
                ParmsDictionary.Add("@rut", rut);
                ParmsDictionary.Add("@dv", dv);
                ParmsDictionary.Add("@nombre", nombre);
                ParmsDictionary.Add("@direccion", direccion);
                ParmsDictionary.Add("@idCmn", idCmn);
                ParmsDictionary.Add("@id", id);

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
