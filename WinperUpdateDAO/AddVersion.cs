using System;
using System.Data.SqlClient;

namespace WinperUpdateDAO
{
    public class AddVersion : SpDao
    {
        public int Execute(string numVersion, DateTime fecVersion, char estado, string comentario, string usuario)
        {
            SpName = @" insert into Versiones (NumVersion, FecVersion, Estado, Comentario, Usuario) values (@numVersion, @fecVersion, @estado, @comentario, @usuario)";
            try
            {
                ParmsDictionary.Add("@numVersion", numVersion);
                ParmsDictionary.Add("@fecVersion", fecVersion);
                ParmsDictionary.Add("@estado", estado);
                ParmsDictionary.Add("@comentario", comentario);
                ParmsDictionary.Add("@usuario", usuario);

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
