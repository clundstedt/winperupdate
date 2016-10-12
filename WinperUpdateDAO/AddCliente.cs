using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddCliente : SpDao
    {
        public int Execute(int rut, char dv, string nombre, string direccion, int idCmn)
        {
            SpName = @" insert into Clientes (Rut, Dv, RazonSocial, Direccion, idCmn) 
                                      values (@rut, @dv, @nombre, @direccion, @idCmn)";
            try
            {
                ParmsDictionary.Add("@rut", rut);
                ParmsDictionary.Add("@dv", dv);
                ParmsDictionary.Add("@nombre", nombre);
                ParmsDictionary.Add("@direccion", direccion);
                ParmsDictionary.Add("@idCmn", idCmn);

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
