using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaFunes : SpDao
    {
        public SqlDataReader Execute(int id)
        {
            try
            {
                SpName =  @"select	a1.*
                            from	Clientes a3,
                                    TrabajadorEmpresaFunes a1,
		                            Funes a2		
                            where	a3.idClientes = @id
                            and     a1.rutEmpresa = convert(varchar(10), a3.rut) + '-' + a3.Dv
                            and		a2.idSolicitud = a1.idSolicitud
                            and		a2.fecha = a1.fecha
                            and		a2.estado = 1";

                ParmsDictionary.Add("@id", id);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
