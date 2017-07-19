using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Suites
    {
        public static List<Model.SuiteBo> GetSuites()
        {
            try
            {
                List<Model.SuiteBo> lista = new List<Model.SuiteBo>();
                var read = new CnaSuites().Execute();
                while (read.Read())
                {
                    lista.Add(new Model.SuiteBo
                    {
                        idSuite = int.Parse(read["idSuite"].ToString()),
                        Nombre = read["Nombre"].ToString(),
                        Subsuites = read["Subsuites"].ToString()
                    });
                }
                read.Close();
                return lista;
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
    }
}
