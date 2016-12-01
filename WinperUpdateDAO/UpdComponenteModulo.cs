using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class UpdComponenteModulo : SpDao
    {
        public int Execute(int idComponentesModulos, string Nombre, string Descripcion, int TipoComponentes)
        {
            SpName = @"UPDATE ComponentesModulos SET Nombre = @Nombre
                                                    ,Descripcion = @Descripcion
                                                    ,TipoComponentes = @TipoComponentes
                                               WHERE idComponentesModulos = @idComponentesModulos";
            try
            {
                ParmsDictionary.Add("@Nombre", Nombre);
                ParmsDictionary.Add("@Descripcion", Descripcion);
                ParmsDictionary.Add("@TipoComponentes", TipoComponentes);
                ParmsDictionary.Add("@idComponentesModulos", idComponentesModulos);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
