using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Version
    {
        public static List<Model.VersionBo> GetVersiones(EventLog log)
        {
            var lista = new List<Model.VersionBo>();
            var consulta = new CnaVersiones();
            try
            {
                var dr = consulta.Execute();
                while (dr.Read())
                {
                    var obj = new Model.VersionBo
                    {
                        IdVersion = int.Parse(dr["idVersion"].ToString()),
                        Release = dr["NumVersion"].ToString(),
                        Fecha = DateTime.Parse(dr["FecVersion"].ToString()),
                        Estado = dr["Estado"].ToString()[0],
                        Componentes = new List<Model.AtributosArchivoBo>()
                    };

                    foreach (var modulo in GetModulosVersiones(obj.IdVersion, null))
                    {
                        foreach (var componente in Componente.GetComponentes(obj.IdVersion, modulo, null))
                        {
                            obj.Componentes.Add(new Model.AtributosArchivoBo {
                                Name = componente.Name,
                                DateCreate = componente.DateCreate,
                                Version = componente.Version,
                                Modulo = componente.Modulo
                            });
                        }
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

        public static List<string> GetModulosVersiones(int idVersion, EventLog log)
        {
            var lista = new List<string>();
            var consulta = new CnaModuloVersiones();
            try
            {
                var dr = consulta.Execute(idVersion);
                while (dr.Read())
                {
                    string obj = dr["Modulo"].ToString().Trim();

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

        public static Model.VersionBo AddVersion(Model.VersionBo version)
        {
            var query = new AddVersion();
            try
            {

                if (query.Execute(version.Release, version.Fecha, version.Estado) > 0)
                {
                    return GetVersiones(null).Last();
                }

                return null;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static int AddModuloVersion(int idVersion, string modulo)
        {
            var query = new AddModuloVersion();
            try
            {
                return query.Execute(idVersion, modulo);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        #region Modelo Antiguo
        public static List<Model.VersionBo> ListarVersiones(string miVersion, string dirVersiones, EventLog log)
        {
            var lista = new List<Model.VersionBo>();
            try {
                var fecVersion = DateTime.Parse(miVersion);

                var xmlDocument = new XmlDocument();
                xmlDocument.Load(dirVersiones + "winper.xml");
                //log.WriteEntry(String.Format("documento cargado: {0}", dirVersiones + "winper.xml"), EventLogEntryType.Information, 0);

                XmlNodeList versiones = xmlDocument.GetElementsByTagName("versiones");
                XmlNodeList listado = ((XmlElement)versiones[0]).GetElementsByTagName("version");
                foreach (XmlElement nodo in listado)
                {
                    XmlNodeList nRelease = nodo.GetElementsByTagName("release");
                    XmlNodeList nFecha = nodo.GetElementsByTagName("fecha");
                    //log.WriteEntry(String.Format("release = {0}, fecha = {1}", nRelease[0].InnerText, nFecha[0].InnerText), EventLogEntryType.Information, 0);

                    var obj = new Model.VersionBo
                    {
                        Release = nRelease[0].InnerText,
                        Fecha = DateTime.Parse(nFecha[0].InnerText)
                    };
                    //log.WriteEntry(String.Format("release obj = {0}, fecha = {1:dd/MM/yyyy}", obj.Release, obj.Fecha), EventLogEntryType.Information, 0);

                    lista.Add(obj);
                }
                return lista.Where(x => x.Fecha.CompareTo(fecVersion) > 0).ToList();
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                log.WriteEntry(msg, EventLogEntryType.Error);
                throw new Exception(msg, ex);
            }

            return lista;
        }

        public static List<Model.AtributosArchivoBo> ListarDirectorio(string directorio, string dirVersiones, EventLog log)
        {
            var lista = new List<Model.AtributosArchivoBo>();
            try
            {
                log.WriteEntry(String.Format("Buscamos archivos en = {0}", dirVersiones + directorio), EventLogEntryType.Information, 0);
                DirectoryInfo di = new DirectoryInfo(dirVersiones + directorio);
                foreach (var fi in di.GetFiles())
                {
                    var obj = new Model.AtributosArchivoBo
                    {
                        Name = fi.Name,
                        DateCreate = fi.CreationTime,
                        LastWrite = fi.LastWriteTime,
                        Length = fi.Length,
                        Version = FileVersionInfo.GetVersionInfo(fi.FullName).FileVersion
                    };

                    lista.Add(obj);
                }

                return lista;
            }
            catch (Exception ex)
            {
                log.WriteEntry("Excepcion Controlada: " + ex.Message, EventLogEntryType.Error);
            }

            return lista;
        }
        #endregion

        public static Byte[] DownloadFile(string namefile, int posIni, int sizeMax, string dirVersiones, EventLog log)
        {
            try
            {
                Byte[] buffer = new Byte[sizeMax]; ;
                string fileName = dirVersiones + namefile;
                log.WriteEntry(String.Format("Download archivo = {0}", fileName), EventLogEntryType.Information, 0);
                int pos = 0;
                int index = 0;
                int letter = 0;
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(stream);

                while (letter != -1 && index < sizeMax)
                {
                    letter = reader.ReadByte();
                    if (letter != -1 && pos >= posIni)
                    {
                        buffer[index++] = (byte)letter;
                    }
                    pos++;
                }
                reader.Close();
                stream.Close();

                log.WriteEntry(String.Format("Bytes leidos = {0}", index), EventLogEntryType.Information, 0);

                return buffer;
            }
            catch (Exception ex)
            {
                log.WriteEntry("Excepcion Controlada: " + ex.Message, EventLogEntryType.Error);
            }

            return null;
        }
    }
}
