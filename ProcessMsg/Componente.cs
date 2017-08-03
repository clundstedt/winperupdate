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
        #region Gets
        public static List<ProcessMsg.Model.AtributosArchivoBo> GetComponentesOficiales(string rutaOficial)
        {
            List<ProcessMsg.Model.AtributosArchivoBo> lista = new List<ProcessMsg.Model.AtributosArchivoBo>();
            try
            {
                var dirs = new System.IO.DirectoryInfo(rutaOficial).GetDirectories().ToList();
                foreach (var dir in dirs)
                {
                    var files = new System.IO.DirectoryInfo(System.IO.Path.Combine(rutaOficial,dir.Name)).GetFiles();
                    files.ToList().ForEach(x =>
                    {
                        lista.Add(new ProcessMsg.Model.AtributosArchivoBo
                        {
                            Name = x.Name,
                            LastWrite = x.LastWriteTime,
                            Version = FileVersionInfo.GetVersionInfo(x.FullName).FileVersion,
                            Length = x.Length,
                            DateCreate = x.CreationTime
                        });
                    });
                }

                return lista.Where(x => x.Version != null).ToList();
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
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
                        Comentario = dr["Comentario"].ToString().Trim(),
                        Tipo = char.Parse(dr["Tipo"].ToString()),
                        MotorSql = dr["MotorSql"].ToString()
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
                        Modulo = dr["Modulo"].ToString().Trim(),
                        Comentario = dr["Comentario"].ToString(),
                        Tipo = char.Parse(dr["Tipo"].ToString()),
                        MotorSql = dr["MotorSql"].ToString()
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
        public static Model.AtributosArchivoBo GetComponenteByName(int idVersion, string nameFile)
        {
            var consulta = new CnaComponenteByName();
            Model.AtributosArchivoBo obj = null;
            try
            {
                var dr = consulta.Execute(idVersion, nameFile);
                while (dr.Read())
                {
                    obj = new Model.AtributosArchivoBo
                    {
                        idVersion = int.Parse(dr["idVersion"].ToString()),
                        Name = dr["NameFile"].ToString(),
                        DateCreate = DateTime.Parse(dr["FechaFile"].ToString()),
                        Version = dr["VersionFile"].ToString(),
                        Modulo = dr["Modulo"].ToString().Trim(),
                        Comentario = dr["Comentario"].ToString().Trim(),
                        Tipo = char.Parse(dr["Tipo"].ToString()),
                        MotorSql = dr["MotorSql"].ToString()
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
        public static List<Model.AtributosArchivoBo> GetComponenteByName(string nameFile)
        {
            var consulta = new CnaComponenteByName();
            var lista = new List<Model.AtributosArchivoBo>();
            try
            {
                var dr = consulta.Execute(nameFile);
                while (dr.Read())
                {
                    lista.Add(new Model.AtributosArchivoBo
                    {
                        idVersion = int.Parse(dr["idVersion"].ToString()),
                        Name = dr["NameFile"].ToString(),
                        DateCreate = DateTime.Parse(dr["FechaFile"].ToString()),
                        Version = dr["VersionFile"].ToString(),
                        Modulo = dr["Modulo"].ToString().Trim(),
                        Comentario = dr["Comentario"].ToString().Trim(),
                        Tipo = char.Parse(dr["Tipo"].ToString()),
                        MotorSql = dr["MotorSql"].ToString()
                    }
                    );
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return lista;
        }
        public static List<Model.AtributosArchivoBo> GetComponenteConDirectorio(int idVersion)
        {
            try
            {
                List<Model.AtributosArchivoBo> lista = new List<Model.AtributosArchivoBo>();
                var r = new CnaComponentes().ExecuteConDirectorio(idVersion);
                while (r.Read())
                {
                    lista.Add(new Model.AtributosArchivoBo
                    {
                        idVersion = int.Parse(r["idVersion"].ToString()),
                        Name = r["NameFile"].ToString(),
                        DateCreate = DateTime.Parse(r["FechaFile"].ToString()),
                        Version = r["VersionFile"].ToString(),
                        Modulo = r["Modulo"].ToString().Trim(),
                        Comentario = r["Comentario"].ToString().Trim(),
                        Directorio = r["Directorio"].ToString(),
                        Tipo = char.Parse(r["Tipo"].ToString()),
                        MotorSql = r["MotorSql"].ToString()
                    });
                }
                r.Close();
                return lista;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        #endregion

        #region Adds
        public static int AddComponente(int idVersion, Model.AtributosArchivoBo componente)
        {
            try
            {
                var lista = Version.GetModulosVersiones(idVersion, null);
                if (!lista.Exists(x => x.Equals(componente.Modulo, StringComparison.OrdinalIgnoreCase)))
                {
                    if (Version.AddModuloVersion(idVersion, componente.Modulo) <= 0) return 0;
                }

                var query = new AddComponente();

                return query.Execute(idVersion, componente.Modulo, componente.Name, componente.Version, componente.LastWrite, componente.Tipo, componente.MotorSql);

            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static object[] AddComponentes(int idVersion, List<ProcessMsg.Model.AtributosArchivoBo> componentes)
        {
            try
            {
                string xml = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\" ?><root>";
                componentes.ForEach(c =>
                {
                    xml += string.Format("<parametro Modulo=\"{0}\" idVersion=\"{1}\" NameFile=\"{2}\" FechaFile=\"{3}\"/>", c.Modulo, idVersion, c.Name, c.DateCreateXml);
                });
                xml += "</root>";
                object[] respuesta = new object[2];
                var reader = new AddComponente().Execute(xml);
                while (reader.Read())
                {
                    respuesta[0] = reader["coderr"].ToString();
                    respuesta[1] = reader["msgerr"].ToString();
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        #endregion

        #region Upds
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
        #endregion

        #region Dels
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
        #endregion

    }
}
