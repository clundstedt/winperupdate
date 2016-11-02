using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;
using System.Diagnostics;

namespace ProcessMsg
{
    public class Componente
    {
        public static List<Model.AtributosArchivoBo> GetComponentes(int idVersion, EventLog log)
        {
            var lista = new List<Model.AtributosArchivoBo>();
            var consulta = new CnaComponentes();
            try
            {
                var dr = consulta.Execute(idVersion);
                while (dr.Read())
                {
                    var obj = new Model.AtributosArchivoBo
                    {
                        Name = dr["NameFile"].ToString(),
                        DateCreate = DateTime.Parse(dr["FechaFile"].ToString()),
                        Version = dr["VersionFile"].ToString(),
                        Modulo = dr["Modulo"].ToString().Trim(),
                        Comentario = dr["Comentario"].ToString().Trim()
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

        public static List<Model.AtributosArchivoBo> GetComponentes(int idVersion, string modulo, EventLog log)
        {
            var lista = new List<Model.AtributosArchivoBo>();
            var consulta = new CnaComponentes();
            try
            {
                var dr = consulta.Execute(idVersion, modulo);
                while (dr.Read())
                {
                    var obj = new Model.AtributosArchivoBo
                    {
                        Name = dr["NameFile"].ToString(),
                        DateCreate = DateTime.Parse(dr["FechaFile"].ToString()),
                        Version = dr["VersionFile"].ToString(),
                        Modulo = dr["Modulo"].ToString().Trim()
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


        public static Model.AtributosArchivoBo AddComponente(int idVersion, Model.AtributosArchivoBo componente)
        {
            try
            {
                var lista = Version.GetModulosVersiones(idVersion, null);
                bool existe = false;
                for (int i = 0; i < lista.Count(); i++)
                {
                    if (lista[i].Trim().ToLower().Equals(componente.Modulo.Trim().ToLower())) existe = true;
                }
                if (!existe)
                {
                    if (Version.AddModuloVersion(idVersion, componente.Modulo) <= 0) return null;
                }

                var query = new AddComponente();

                if (query.Execute(idVersion, componente.Modulo, componente.Name, componente.Version, componente.LastWrite) > 0)
                {
                    return GetComponentes(idVersion, componente.Modulo, null).Last();
                }

                return null;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static Model.AtributosArchivoBo UpdComponente(int idVersion, Model.AtributosArchivoBo componente)
        {
            try
            {
                var lista = Version.GetModulosVersiones(idVersion, null);
                if (!lista.Contains(componente.Modulo))
                {
                    if (Version.AddModuloVersion(idVersion, componente.Modulo) <= 0) return null;
                }

                var query = new UpdComponente();

                if (query.Execute(idVersion, componente.Name, componente.Modulo, componente.Comentario) > 0)
                {
                    return GetComponenteByName(idVersion, componente.Name);
                }

                return null;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static int DelComponente(int idVersion, Model.AtributosArchivoBo componente)
        {
            try
            {
                var query = new DelComponente();

                return query.Execute(idVersion, componente.Name);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static Model.AtributosArchivoBo GetComponenteByName(int idVersion, string nameFile)
        {
            var consulta = new CnaComponenteByName();
            var obj = new Model.AtributosArchivoBo();
            try
            {
                var dr = consulta.Execute(idVersion, nameFile);
                while (dr.Read())
                {
                    obj = new Model.AtributosArchivoBo
                    {
                        Name = dr["NameFile"].ToString(),
                        DateCreate = DateTime.Parse(dr["FechaFile"].ToString()),
                        Version = dr["VersionFile"].ToString(),
                        Modulo = dr["Modulo"].ToString().Trim(),
                        Comentario = dr["Comentario"].ToString().Trim()
                    };
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return obj;
        }
    }
}
