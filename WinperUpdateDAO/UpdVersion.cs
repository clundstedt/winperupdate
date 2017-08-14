using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class UpdVersion : SpDao
    {
        public int Execute(int idVersion, string numVersion, DateTime fecVersion, char estado, string comentario, string usuario, string instalador, bool hasdeploy)
        {
            SpName = @" update Versiones 
                           set NumVersion = @numVersion,
                               FecVersion = @fecVersion,
                               Estado     = @estado,
                               Comentario = @comentario,
                               Usuario    = @usuario,
                               Instalador = @instalador,
                               HasDeploy31 = @hasdeploy
                         where idVersion  = @idVersion";
            try
            {
                ParmsDictionary.Add("@numVersion", numVersion);
                ParmsDictionary.Add("@fecVersion", fecVersion);
                ParmsDictionary.Add("@estado", estado);
                ParmsDictionary.Add("@comentario", comentario);
                ParmsDictionary.Add("@usuario", usuario);
                ParmsDictionary.Add("@instalador", instalador);
                ParmsDictionary.Add("@idVersion", idVersion);
                ParmsDictionary.Add("@hasdeploy", hasdeploy);

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
