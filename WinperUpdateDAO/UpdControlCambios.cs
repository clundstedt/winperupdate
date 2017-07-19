using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class UpdControlCambios : SpDao
    {
        public bool Execute(int tips, int version, int modulo, string release, string descripcion
            , DateTime fecha, string impacto, string[] files, int[] idClientes, string usuario)
        {
            try
            {
                int contFila = 0;
                object[,] obj = new object[files.Length + idClientes.Length + 1, 2];
                SpName = @"UPDATE controlcambios SET Release = @release, Descripcion = @descripcion, Fecha = @fecha, Impacto = @impacto WHERE Tips = @tips AND Version = @version AND Modulo = @modulo";
                ParmsDictionary.Add("@tips", tips);
                ParmsDictionary.Add("@version", version);
                ParmsDictionary.Add("@modulo", modulo);
                ParmsDictionary.Add("@release", release);
                ParmsDictionary.Add("@descripcion", descripcion);
                ParmsDictionary.Add("@fecha", fecha);
                ParmsDictionary.Add("@impacto", impacto);
                obj[contFila, 0] = SpName;
                obj[contFila, 1] = ParmsDictionary;
                for (int i = 0; i < files.Length; i++)
                {
                    contFila++;
                    string sqlDoc = @"INSERT INTO doccambios (Nombre, VersionCC, ModuloCC, TipsCC) VALUES ({0},{1},{2},{3})";
                    string[] dataDoc =
                    {
                            "@nombre"+contFila,
                            "@versioncc"+contFila,
                            "@modulocc"+contFila,
                            "@tipscc"+contFila
                        };
                    var parms = new ConnectorDB.ThDictionary();
                    parms.Add(dataDoc[0], files[i]);
                    parms.Add(dataDoc[1], version);
                    parms.Add(dataDoc[2], modulo);
                    parms.Add(dataDoc[3], tips);
                    obj[contFila, 0] = string.Format(sqlDoc, dataDoc[0], dataDoc[1], dataDoc[2], dataDoc[3]);
                    obj[contFila, 1] = parms;
                    for (int j = 0; j < idClientes.Length; j++)
                    {
                        contFila++;
                        string sqlCl = @"INSERT INTO clientes_has_doccambios (Cliente, NombreDC, VersionCC, ModuloCC, TipsCC) VALUES ({0},{1},{2},{3},{4})";
                        string[] dataCl =
                        {
                            "@cliente"+contFila,
                            "@nombredc"+contFila,
                            "@versioncc"+contFila,
                            "@modulocc"+contFila,
                            "@tipscc"+contFila
                        };
                        var parmsCl = new ConnectorDB.ThDictionary();
                        parmsCl.Add(dataCl[0], idClientes[j]);
                        parmsCl.Add(dataCl[1], files[i]);
                        parmsCl.Add(dataCl[2], version);
                        parmsCl.Add(dataCl[3], modulo);
                        parmsCl.Add(dataCl[4], tips);
                        obj[contFila, 0] = string.Format(sqlCl, dataCl[0], dataCl[1], dataCl[2], dataCl[3], dataCl[4]);
                        obj[contFila, 1] = parmsCl;
                    }
                }
                return Connector.ExecuteQueryTrans(obj);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}
