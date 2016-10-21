using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class DelAmbiente : SpDao
    {
        public int Execute(int idAmbiente, int idCliente)
        {
            SpName = @"DELETE FROM ambientes WHERE idAmbientes = @idAmbiente AND idClientes = @idCliente";
            try
            {
                ParmsDictionary.Add("@idAmbiente", idAmbiente);
                ParmsDictionary.Add("@idCliente", idCliente);

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
