using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Modulo
    {
        public static List<Model.ModuloBo> GetModulos(EventLog log)
        {
            var lista = new List<Model.ModuloBo>();
            var consulta = new CnaModulo();
            try
            {
                var dr = consulta.Execute();
                while (dr.Read())
                {
                    var obj = new Model.ModuloBo
                    {
                        idModulo = int.Parse(dr["idModulo"].ToString()),
                        NomModulo = dr["NomModulo"].ToString()
                    };

                    lista.Add(obj);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                if (log != null) log.WriteEntry(msg, EventLogEntryType.Error);
                throw new Exception(msg, ex);
            }

            return lista;
        }
    }
}
