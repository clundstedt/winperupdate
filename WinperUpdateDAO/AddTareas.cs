using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class AddTareas : SpDao
    {
        public int Execute(int idTareas, int idClientes, int idAmbientes, int CodPrf
                          ,int Estado, string Modulo, int idVersion, string NameFile
                          ,string Error)
        {
            SpName = @"INSERT INTO tareas (idClientes
                                          ,idAmbientes
                                          ,CodPrf
                                          ,Estado
                                          ,Modulo
                                          ,idVersion
                                          ,NameFile
                                          ,Error)
                                    VALUES(@idClientes
                                          ,@idAmbientes
                                          ,@CodPrf
                                          ,@Estado
                                          ,@Modulo
                                          ,@idVersion
                                          ,@NameFile
                                          ,@Error)";
            try
            {
                ParmsDictionary.Add("@idClientes",idClientes);
                ParmsDictionary.Add("@idAmbientes",idAmbientes);
                ParmsDictionary.Add("@CodPrf",CodPrf);
                ParmsDictionary.Add("@Estado",Estado);
                ParmsDictionary.Add("@Modulo",Modulo);
                ParmsDictionary.Add("@idVersion",idVersion);
                ParmsDictionary.Add("@NameFile",NameFile);
                ParmsDictionary.Add("@Error",Error);

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
