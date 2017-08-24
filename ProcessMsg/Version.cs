using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Version
    {
        #region Gets

        public static List<Model.ControlCambiosBo> GetControlCambios(int idVersion)
        {
            try
            {
                List<Model.ControlCambiosBo> lista = new List<Model.ControlCambiosBo>();
                var r = new CnaControlCambios().ExecuteByVersion(idVersion);
                while (r.Read())
                {
                    lista.Add(new Model.ControlCambiosBo
                    {
                        Tips = int.Parse(r["tips"].ToString()),
                        Version = int.Parse(r["version"].ToString()),
                        Modulo = int.Parse(r["modulo"].ToString()),
                        Release = r["release"].ToString(),
                        Descripcion = r["descripcion"].ToString(),
                        Fecha = Convert.ToDateTime(r["fecha"].ToString()),
                        Impacto = r["impacto"].ToString(),
                        Usuario = r["usuario"].ToString(),
                        ModuloFmt = Modulo.GetModulo(int.Parse(r["modulo"].ToString())),
                        VersionFmt = Version.GetVersion(int.Parse(r["version"].ToString())),
                        DocCambios = GetDocCambiosByVersionAndTips(int.Parse(r["version"].ToString()), int.Parse(r["tips"].ToString()))
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

        public static List<string> GetDocCambiosByVersionAndTips(int idVersion, int tips)
        {
            try
            {
                List<string> lista = new List<string>();
                var r = new CnaControlCambios().ExecuteDocCambiosByVersionAndTips(idVersion, tips);
                while (r.Read())
                {
                    lista.Add(r["Nombre"].ToString());
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

        public static Model.VersionBo GetVersion(int idVersion)
        {
            try
            {
                Model.VersionBo obj = new Model.VersionBo();
                var r = new CnaVersiones().Execute(idVersion);
                while (r.Read())
                {
                    obj = new Model.VersionBo
                    {
                        IdVersion = int.Parse(r["idVersion"].ToString()),
                        Release = r["NumVersion"].ToString(),
                        Fecha = DateTime.Parse(r["FecVersion"].ToString()),
                        Estado = r["Estado"].ToString()[0],
                        Comentario = r["Comentario"].ToString(),
                        Usuario = r["Usuario"].ToString(),
                        Instalador = r["Instalador"].ToString(),
                        Componentes = new List<Model.AtributosArchivoBo>(),
                        IsVersionInicial = bool.Parse(r["IsVersionInicial"].ToString()),
                        HasDeploy31 = bool.Parse(r["HasDeploy31"].ToString())
                    };
                }
                r.Close();
                return obj;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static Model.VersionToClienteBo CheckVersionAnteriorInstalada(int idVersion, int idCliente, int idAmbiente)
        {
            try
            {
                var res = new CnaVersionesCliente().CheckVersionAnteriorInstalada(idVersion, idCliente, idAmbiente);
                while (res.Read())
                {
                    return new Model.VersionToClienteBo
                    {
                        Cliente = new Model.ClienteBo { Id = int.Parse(res["idClientes"].ToString()) },
                        Version = new Model.VersionBo { IdVersion = int.Parse(res["idVersion"].ToString()) },
                        Ambiente = new Model.AmbienteBo { idAmbientes = int.Parse(res["idAmbientes"].ToString()) },
                        Estado = char.Parse(res["Estado"].ToString()),
                        FechaInstalacion = Convert.ToDateTime(res["FechaInstalacion"].ToString())
                    };
                }
                res.Close();
                return null;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static List<Model.VersionToClienteBo> GetVersionesToCliente(int idCliente)
        {
            try
            {
                List<Model.VersionToClienteBo> lista = new List<Model.VersionToClienteBo>();
                var reader = new CnaVersionesCliente().ExecuteVersionesCliente(idCliente);
                while (reader.Read())
                {
                    lista.Add(new Model.VersionToClienteBo
                    {
                        Cliente = new Model.ClienteBo { Id = int.Parse(reader["idClientes"].ToString()) },
                        Version = new Model.VersionBo
                        {
                            IdVersion = int.Parse(reader["idVersion"].ToString()),
                            Release = reader["NumVersion"].ToString()
                        },
                        Ambiente = new Model.AmbienteBo
                        {
                            idAmbientes = int.Parse(reader["idAmbientes"].ToString()),
                            Nombre = reader["nombre"].ToString()
                        },
                        Estado = char.Parse(reader["Estado"].ToString()),
                        FechaInstalacion = Convert.ToDateTime(reader["FechaInstalacion"].ToString())
                    });
                }
                reader.Close();
                return lista;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
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
                        Comentario = dr["Comentario"].ToString(),
                        Usuario = dr["Usuario"].ToString(),
                        Instalador = dr["Instalador"].ToString(),
                        Componentes = new List<Model.AtributosArchivoBo>(),
                        IsVersionInicial = bool.Parse(dr["IsVersionInicial"].ToString()),
                        HasDeploy31 = bool.Parse(dr["HasDeploy31"].ToString())
                    };

                    foreach (var modulo in GetModulosVersiones(obj.IdVersion, null))
                    {
                        foreach (var componente in Componente.GetComponentes(obj.IdVersion, modulo, null))
                        {
                            obj.Componentes.Add(new Model.AtributosArchivoBo
                            {
                                Name = componente.Name,
                                DateCreate = componente.DateCreate,
                                Version = componente.Version,
                                Modulo = componente.Modulo,
                                Comentario = componente.Comentario
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
        public static Model.VersionBo GetVersion(string release, EventLog log)
        {
            Model.VersionBo obj = null;
            var consulta = new CnaVersiones();
            try
            {
                var dr = consulta.Execute(release);
                if (dr.Read())
                {
                    obj = new Model.VersionBo
                    {
                        IdVersion = int.Parse(dr["idVersion"].ToString()),
                        Release = dr["NumVersion"].ToString(),
                        Fecha = DateTime.Parse(dr["FecVersion"].ToString()),
                        Estado = dr["Estado"].ToString()[0],
                        Comentario = dr["Comentario"].ToString(),
                        Usuario = dr["Usuario"].ToString(),
                        Instalador = dr["Instalador"].ToString(),
                        Componentes = new List<Model.AtributosArchivoBo>(),
                        IsVersionInicial = bool.Parse(dr["IsVersionInicial"].ToString()),
                        HasDeploy31 = bool.Parse(dr["HasDeploy31"].ToString())
                    };

                    foreach (var modulo in GetModulosVersiones(obj.IdVersion, null))
                    {
                        foreach (var componente in Componente.GetComponentes(obj.IdVersion, modulo, null))
                        {
                            obj.Componentes.Add(new Model.AtributosArchivoBo
                            {
                                Name = componente.Name,
                                DateCreate = componente.DateCreate,
                                Version = componente.Version,
                                Modulo = componente.Modulo
                            });
                        }
                    };

                }
                dr.Close();

                return obj;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                if (log != null) log.WriteEntry(msg, EventLogEntryType.Error);
                throw new Exception(msg, ex);
            }

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

                if (query.Execute(version.Release, version.Fecha, version.Estado, version.Comentario, version.Usuario, version.IsVersionInicial, version.HasDeploy31) > 0)
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

        public static Model.VersionBo UpdVersion(int idVersion, Model.VersionBo version)
        {
            var query = new UpdVersion();
            try
            {

                if (query.Execute(idVersion, version.Release, version.Fecha, version.Estado, version.Comentario, version.Usuario, version.Instalador, version.HasDeploy31) > 0)
                {
                    return GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
                }

                return null;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static Model.VersionBo UpdEstadoVersion(int idVersion, Model.VersionBo version)
        {
            var query = new PubVersion();
            try
            {

                if (query.Execute(idVersion, version.Estado) > 0)
                {
                    return GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
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

        #region Adds
        public static bool AddControlCambios(Model.ControlCambiosBo ctrlCambios)
        {
            try
            {
                var mod = ProcessMsg.Modulo.GetModulo(ctrlCambios.Modulo);
                if (mod != null)
                {
                    var idClientes = ProcessMsg.Cliente.GetClientesByModulo(ctrlCambios.Modulo).Select(x => x.Id).ToArray();
                    
                    return new AddControlCambios().Execute(ctrlCambios.Tips, ctrlCambios.Version, ctrlCambios.Modulo, ctrlCambios.Release, ctrlCambios.Descripcion
                        , ctrlCambios.Fecha, ctrlCambios.Impacto, ctrlCambios.DocCambios.ToArray(), idClientes, ctrlCambios.Usuario);
                }
                return false;
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
        public static int AddVersionAmbiente(int idVersion, int idAmbiente, int idCliente, char estado)
        {
            var query = new AddVersionToAmbiente();
            try
            {
                return query.Execute(idCliente, idVersion, idAmbiente, estado);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static int AddVersionCliente(int idVersion, int idClientes)
        {
            var query = new AddVersionCliente();
            try
            {
                return query.Execute(idVersion, idClientes);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static object[] AddVersionCliente(int idVersion, List<int> idClientes)
        {
            try
            {
                string xml = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\" ?><root>";
                idClientes.ForEach(x =>
                {
                    xml += string.Format("<parametro idVersion=\"{0}\" idClientes=\"{1}\"/>", idVersion, x);
                });
                xml += "</root>";
                object[] respuesta = new object[2];
                var reader = new AddVersionCliente().Execute(xml);
                while (reader.Read())
                {
                    respuesta[0] = reader["coderr"].ToString();
                    respuesta[1] = reader["msgerr"].ToString();
                }
                reader.Close();
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
        public static bool UpdControlCambios(Model.ControlCambiosBo ctrlCambios)
        {
            try
            {
                var mod = ProcessMsg.Modulo.GetModulo(ctrlCambios.Modulo);
                if (mod != null)
                {
                    var idClientes = ProcessMsg.Cliente.GetClientesByModulo(ctrlCambios.Modulo).Select(x => x.Id).ToArray();

                    return new UpdControlCambios().Execute(ctrlCambios.Tips, ctrlCambios.Version, ctrlCambios.Modulo, ctrlCambios.Release, ctrlCambios.Descripcion
                        , ctrlCambios.Fecha, ctrlCambios.Impacto
                        , ctrlCambios.DocCambios == null ? new string[] { } : ctrlCambios.DocCambios.ToArray()
                        , ctrlCambios.DocCambios == null ? new int[] { } : idClientes, ctrlCambios.Usuario);
                }
                return false;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        #endregion

        #region Dels
        public static int DelVersion(int idVersion)
        {
            var query = new DelVersion();
            try
            {
                return query.Execute(idVersion);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static int DelDocCambios(string dirFuente, string doc, int version, int tips, int modulo)
        {
            try
            {
                if (new DelControlCambios().ExecuteDocCambios(doc, version,tips,modulo) == 1)
                {
                    
                    var pathTxt = Path.Combine(dirFuente
                        , "DocCambios"
                        , string.Format("{0}_{1}_{2}", version, modulo, tips)
                        ,doc);
                    File.Delete(pathTxt);
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static int DelControlCambios(string dirFuente, int version, int tips, int modulo)
        {
            try
            {
                if (new DelControlCambios().Execute(version, tips, modulo) == 1)
                {
                    var pathTxt = Path.Combine(dirFuente
                       , "DocCambios"
                       , string.Format("{0}_{1}_{2}", version, modulo, tips));
                    Directory.Delete(pathTxt, true);
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        #endregion

        #region Otros

        public static bool GenerarControlCambios(int idVersion, string pathVoN1)
        {
            try
            {
                var listaCtrlCambios = ProcessMsg.Version.GetControlCambios(idVersion).Where(x => !x.VersionFmt.IsVersionInicial).OrderBy(x => x.Modulo).ToList();
                if (listaCtrlCambios.Count > 0)
                {
                    byte process = 0;
                    do
                    {
                        if (process == 0)
                        {
                            foreach (var idModulo in listaCtrlCambios.Select(x => x.Modulo).Distinct())
                            {
                                var txtInfo = ProcessMsg.ComponenteModulo.GetComponentesConDirectorioByModulo(idModulo).SingleOrDefault(x => x.TipoComponentes.isCompCambios);
                                if (txtInfo != null)
                                {
                                    var pathTxt = Path.Combine(pathVoN1, txtInfo.Directorio, txtInfo.Nombre);
                                    if (!File.Exists(pathTxt)) throw new Exception(string.Format("El control de cambios {0} no existe.", pathTxt));
                                }
                            }
                        }
                        ; if (process == 1)
                        {
                            foreach (var idModulo in listaCtrlCambios.Select(x => x.Modulo).Distinct())
                            {
                                var txtInfo = ProcessMsg.ComponenteModulo.GetComponentesConDirectorioByModulo(idModulo).SingleOrDefault(x => x.TipoComponentes.isCompCambios);
                                if (txtInfo != null)
                                {
                                    var pathTxt = Path.Combine(pathVoN1, txtInfo.Directorio, txtInfo.Nombre);
                                    var pathTxtAnt = Path.Combine(pathVoN1, txtInfo.Directorio, "Control_Cambios_Anterior");
                                    if (!Directory.Exists(pathTxtAnt))
                                    {
                                        Directory.CreateDirectory(pathTxtAnt);
                                    }
                                    File.Copy(pathTxt, Path.Combine(pathTxtAnt
                                        , string.Format("{0:ddMMyyyyHHmmss}_{1}", DateTime.Now, txtInfo.Nombre)));
                                }
                            }
                        }
                        ; if (process == 2)
                        {
                            foreach (var ctrlCambios in listaCtrlCambios)
                            {
                                var txtInfo = ProcessMsg.ComponenteModulo.GetComponentesModulos(ctrlCambios.Modulo).SingleOrDefault(x => x.TipoComponentes.isCompCambios);
                                if (txtInfo != null)
                                {
                                    var pathTxt = Path.Combine(pathVoN1, ctrlCambios.ModuloFmt.Directorio, txtInfo.Nombre);
                                    string[] info = {
                                string.Format("Fecha: {0:dd/MM/yyyy}", ctrlCambios.Fecha),
                                string.Format("Nº Tips: {0}",ctrlCambios.Tips),
                                string.Format("Archivo: {0}",string.Join(", ", ctrlCambios.DocCambios.ToArray())),
                                string.Format("Release de Mantenimiento: {0}",ctrlCambios.Release),
                                string.Format("Descripcion: {0}",ctrlCambios.Descripcion),
                                string.Format("Impacto Modificación: {0}",ctrlCambios.Impacto),
                                "*****************************************************************"
                            };
                                    List<string> bufferFinal = new List<string>();
                                    var buffer = File.ReadAllLines(pathTxt, Encoding.GetEncoding("ISO-8859-1")).ToList();
                                    bufferFinal.AddRange(info);
                                    bufferFinal.AddRange(buffer);

                                    File.WriteAllLines(pathTxt, bufferFinal.ToArray());

                                }
                            }
                        }
                        process++;
                    } while (process < 3);
                    return (process == 3);
                }
                return false;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static int GenerarInstalador(int idVersion, string fileVersion, string dirSource, string dirN1, string dirFuentes)
        {
            try
            {
                var version = GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
               
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(dirSource + fileVersion + ".iss", false, Encoding.GetEncoding("ISO-8859-1")))
                {
                    file.WriteLine("#define MyAppName \"WinPer\"");
                    file.WriteLine("#define MyAppVersion \"" + version.Release + "\"");
                    file.WriteLine("#define MyAppPublisher \"INNOVASOFT.\"");
                    file.WriteLine("#define MyAppURL \"http://www.innovasoft.cl\"");
                    file.WriteLine("#define MyOutputDir \"" + dirSource + "Output\"");
                    file.WriteLine("#define MyOutputBaseFilename \"" + fileVersion + "\"");
                    file.WriteLine(@"");
                    file.WriteLine(@"[Setup]");
                    file.WriteLine(@"AppId={{" + idVersion + "}}");
                    file.WriteLine(@"AppName={#MyAppName}");
                    file.WriteLine(@"AppVersion={#MyAppVersion}");
                    file.WriteLine(@"AppPublisher={#MyAppPublisher}");
                    file.WriteLine(@"AppPublisherURL={#MyAppURL}");
                    file.WriteLine(@"AppSupportURL={#MyAppURL}");
                    file.WriteLine(@"AppUpdatesURL={#MyAppURL}");
                    file.WriteLine(@"DefaultDirName={localappdata}\WinperSetupUI\{#MyAppVersion}");
                    file.WriteLine(@"DisableDirPage=yes");
                    file.WriteLine(@"DefaultGroupName={#MyAppName}");
                    file.WriteLine(@"DisableProgramGroupPage=yes");
                    file.WriteLine(@"OutputDir={#MyOutputDir}");
                    file.WriteLine(@"OutputBaseFilename={#MyOutputBaseFilename}");
                    file.WriteLine(@"Compression=lzma");
                    file.WriteLine(@"SolidCompression=yes");
                    file.WriteLine(@"");
                    file.WriteLine(@"[Languages]");
                    file.WriteLine("Name: \"spanish\"; MessagesFile: \"compiler:Languages\\Spanish.isl\"");
                    file.WriteLine(@"");
                    file.WriteLine(@"[Files]");
                    foreach (var componente in version.Componentes)
                    {
                        if(componente.Extension.Equals(".sql", StringComparison.OrdinalIgnoreCase))
                            file.WriteLine(string.Format("Source: \"{0}\"; DestDir: \"{{app}}\"", dirSource + "\\Scripts\\" + componente.Name));
                        else
                        file.WriteLine(string.Format("Source: \"{0}\"; DestDir: \"{{app}}\"", dirSource + componente.Name));
                    }
                    if (!version.IsVersionInicial)
                    {
                        var modsVersion = ProcessMsg.Version.GetModulosVersiones(idVersion, null);
                        var mods = ProcessMsg.Modulo.GetModulos(null).Where(x => modsVersion.Exists(y => y.Equals(x.NomModulo, StringComparison.OrdinalIgnoreCase))).ToList();
                        var compMod = ProcessMsg.ComponenteModulo.GetComponentesModulos().Where(x => mods.Exists(y => y.idModulo == x.Modulo) && x.TipoComponentes.isCompCambios).ToList();
                        foreach (var txt in compMod)
                        {
                            file.WriteLine(string.Format("Source: \"{0}\"; DestDir: \"{{app}}\"", Path.Combine(dirN1, mods.SingleOrDefault(x => x.idModulo == txt.Modulo).Directorio, txt.Nombre)));
                        }
                    }
                    if (version.HasDeploy31)
                    {
                        file.WriteLine(string.Format("Source: \"{0}\"; DestDir: \"{{app}}\"", Path.Combine(dirFuentes, "Utils", "Deploy31.exe")));
                    }
                    file.WriteLine(@"");
                    file.WriteLine(@"[Icons]");
                    file.WriteLine(@"Name: ""{group}\WinPer""; Filename: ""{app}\reconect.exe""");
                    file.WriteLine(@"");

                    file.WriteLine(@"[Dirs]");
                    file.WriteLine(@"Name: ""{app}\info""");
                    file.WriteLine(@"Name: ""{app}\plantillas_certificados""");
                    file.WriteLine(@"Name: ""{app}\plantillas_contratos""");
                    file.WriteLine(@"Name: ""{app}\plantillas_finiquitos""");
                    file.WriteLine(@"Name: ""{app}\plantillas_papeletas_rem""");

                    file.WriteLine(@"");
                    file.WriteLine(@"[Registry]");
                    //file.WriteLine(@"Root: HKCU; Subkey: ""SOFTWARE\WinperUpdate""; ValueType: string; ValueName: ""Version""; ValueData: """ + version.Release + "\"");
                    file.WriteLine(@"Root: HKCU; Subkey: ""SOFTWARE\WinperUpdate""; ValueType: string; ValueName: ""Status""; ValueData: """ + "begin" + "\"");

                    file.Close();
                }

                return 1;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        /// <summary>
        /// Copia un directorio en la ruta de destino
        /// </summary>
        /// <param name="dirOrigen">directorio a copiar</param>
        /// <param name="dirDestino">directorio de destino</param>
        /// <param name="copiarSubDirs">especifica si se quieren copiar los subdirectorios</param>
        /// <param name="overWrite">especifica si se quieren sobreescribir los archivos</param>
        public static void CopiarDirectorio(string dirOrigen, string dirDestino, bool copiarSubDirs, bool overWrite)
        {
            DirectoryInfo dir = new DirectoryInfo(dirOrigen);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "El directorio no existe: "
                    + dirOrigen);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(dirDestino))
            {
                Directory.CreateDirectory(dirDestino);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {

                string temppath = Path.Combine(dirDestino, file.Name);
                file.CopyTo(temppath, overWrite);
            }

            if (copiarSubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(dirDestino, subdir.Name);
                    CopiarDirectorio(subdir.FullName, temppath, copiarSubDirs, overWrite);
                }
            }
        }
        #region Modelo Antiguo
        public static List<Model.VersionBo> ListarVersiones(string miVersion, string dirVersiones, EventLog log)
        {
            var lista = new List<Model.VersionBo>();
            try
            {
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
                string fileName = "";
                if (new FileInfo(namefile).Extension.Equals(".sql", StringComparison.OrdinalIgnoreCase))
                {
                    var sp = namefile.Split('\\');
                    fileName = Path.Combine(dirVersiones,sp[0], "Scripts", sp[1]);
                }
                else
                {
                    fileName = dirVersiones + namefile;
                }
                long sizeBuffer = sizeMax;
                var sizeFile = new FileInfo(fileName).Length;
                if (sizeFile - posIni < sizeMax)
                {
                    sizeBuffer = sizeFile - posIni;
                }
                Byte[] buffer = new Byte[sizeBuffer];
                log.WriteEntry(String.Format("Download archivo = {0}", fileName), EventLogEntryType.Information, 0);
                int pos = 0;
                int index = 0;
                int letter = 0;
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(stream);

                while (letter != -1 && index < sizeBuffer)
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
        #endregion

    }
}
