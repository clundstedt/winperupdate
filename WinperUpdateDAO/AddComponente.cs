using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddComponente : SpDao
    {
        public int Execute(int idVersion, string modulo, string nameFile, string numVersion, DateTime fecVersion)
        {
            SpName = @" insert into Componentes (idVersion, Modulo, NameFile, VersionFile, FechaFile, Comentario) 
                                    values (@idVersion, @modulo, @nameFile, @numVersion, @fecVersion, '')";
            try
            {
                ParmsDictionary.Add("@idVersion", idVersion);
                ParmsDictionary.Add("@modulo", modulo);
                ParmsDictionary.Add("@nameFile", nameFile);
                ParmsDictionary.Add("@numVersion", numVersion);
                ParmsDictionary.Add("@fecVersion", fecVersion);

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
