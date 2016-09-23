using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class UpdComponente : SpDao
    {
        public int Execute(int idVersion, string nameFile, string modulo, string comentario)
        {
            SpName = @" update Componentes 
                           set Modulo = @modulo,
                               Comentario = @comentario
                         where idVersion = @idVersion
                           and NameFile  = @nameFile";
            try
            {
                ParmsDictionary.Add("@modulo", modulo);
                ParmsDictionary.Add("@comentario", comentario);
                ParmsDictionary.Add("@idVersion", idVersion);
                ParmsDictionary.Add("@nameFile", nameFile);

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
