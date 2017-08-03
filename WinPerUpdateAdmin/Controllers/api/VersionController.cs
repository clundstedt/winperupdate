using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class VersionController : ApiController
    {

        #region Clases
        public class ClienteToVersion
        {
            public bool check { get; set; }
            public ProcessMsg.Model.ClienteBo cliente { get; set; }
        }

        public class ComponenteOk
        {
            public string isOk { get; set; }
            public ProcessMsg.Model.AtributosArchivoBo componente { get; set; }
        }
        #endregion

        #region get
        
        [Route("api/Version/{idVersion:int}/{idCliente:int}/{idAmbiente:int}/ScriptsOk")]
        [HttpGet]
        public Object GetScriptsOk(int idVersion, int idCliente, int idAmbiente)  
        {
            try
            {
                return Content(HttpStatusCode.OK, ProcessMsg.Tareas.GetScriptsOk(idVersion, idCliente,idAmbiente));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Version/{idVersion:int}/HasScript")]
        [HttpGet]
        public Object GetHasScript(int idVersion)
        {
            try
            {
                var obj = ProcessMsg.Componente.GetComponentes(idVersion, null).Where(x => x.Tipo != '*').ToList();
                return Content(HttpStatusCode.OK, (obj.Count > 0));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/getControlCambios/{idVersion:int}/{tips:int}/{modulo:int}")]
        [HttpGet]
        public Object GetControlCambios(int idVersion, int tips, int modulo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return Content(HttpStatusCode.OK, ProcessMsg.Version.GetControlCambios(idVersion).SingleOrDefault(x => x.Tips==tips && x.Modulo==modulo));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/getControlCambios/{idVersion:int}")]
        [HttpGet]
        public Object GetControlCambios(int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return Content(HttpStatusCode.OK, ProcessMsg.Version.GetControlCambios(idVersion).OrderByDescending(x => x.Fecha));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        [Route("api/ComponenteOkSegunVersion/{idVersion:int}")]
        [HttpGet]
        public Object ComponenteOkSegunVersion(int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var version = ProcessMsg.Version.GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
                if (version == null)
                {
                    return Content(HttpStatusCode.BadRequest, (Object)null);
                }
                List<ComponenteOk> componentesOk = new List<ComponenteOk>();
                var componentesOficiales = ProcessMsg.Componente.GetComponentesOficiales(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/VersionOficial/"))+ "N+1");
                foreach (var comp in version.Componentes)
                {
                    ComponenteOk cOk = new ComponenteOk();
                    cOk.componente = comp;
                    cOk.isOk = "success";
                    foreach (var compOficial in componentesOficiales)
                    {
                        if (comp.Name.Equals(compOficial.Name))
                        {
                            try
                            {
                                var versionBase = ProcessMsg.Utils.GenerarVersionSiguiente(compOficial.Version);
                                var versionOtra = comp.Version;
                                var comparacion = ProcessMsg.Utils.ComparaVersion(versionBase, versionOtra);
                                if (comparacion == 0)
                                {
                                    cOk.isOk = "success";
                                }
                                else if (comparacion == 1)
                                {
                                    cOk.componente.MensajeToolTip = "WinperUpdate ha detectado que la versión de este componente es " + cOk.componente.Version + " y debiera ser " + versionBase + ", ya que la versión oficial es " + compOficial.Version;
                                    cOk.isOk = "warning";
                                }
                                else
                                {
                                    cOk.componente.MensajeToolTip = "WinperUpdate ha detectado que la versión de este componente (" + cOk.componente.Version + ") debiera ser " + versionBase + ".";
                                    cOk.isOk = "danger";
                                }
                            }
                            catch (Exception) { }
                        }
                    }
                    componentesOk.Add(cOk);
                }
                return Content(HttpStatusCode.OK, componentesOk);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/VersionInicial/{idVersion:int}/Cliente")]
        [HttpGet]
        public Object PostVersionInicialCliente(int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Version.GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
                if (obj != null)
                {
                    var upload = ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Uploads"));
                    var incial = Path.Combine(upload, obj.Release);
                    if (!Directory.Exists(incial))
                    {
                        Directory.CreateDirectory(incial);
                    }
                    else
                    {
                        Directory.Delete(incial, true);
                        Directory.CreateDirectory(incial);
                    }
                    var mods = ProcessMsg.Modulo.GetModulos(null);
                    var comps = new List<ProcessMsg.Model.AtributosArchivoBo>();
                    var CompModulo = ProcessMsg.ComponenteModulo.GetComponentesConDirectorio();
                    DirectoryInfo di = new DirectoryInfo(Path.Combine(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/VersionOficial/")), "N+1"));
                    di.GetDirectories().ToList().ForEach(dir =>
                    {
                        dir.GetFiles().ToList().ForEach(file =>
                        {

                            if ((file.Attributes & FileAttributes.System) != FileAttributes.System)
                            {
                                foreach (var c in CompModulo)
                                {
                                    if (c.Nombre.Equals(file.Name)
                                    && mods.Exists(x => x.idModulo == c.Modulo && x.isCore)
                                    && file.DirectoryName.EndsWith(c.Directorio)
                                    && !comps.Exists(x => x.Name.Equals(file.Name, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        file.CopyTo(Path.Combine(incial, file.Name), true);
                                        comps.Add(new ProcessMsg.Model.AtributosArchivoBo
                                        {
                                            Name = file.Name,
                                            Modulo = c.NomModulo,
                                            DateCreate = file.CreationTime,
                                            LastWrite = file.LastWriteTime
                                        });
                                    }
                                }
                            }
                        });
                    });
                    var respuesta = ProcessMsg.Componente.AddComponentes(obj.IdVersion, comps);
                    if (respuesta[0].Equals("0"))
                    {
                        bool okInstall = false;

                        string sRuta = ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Uploads/")) + obj.Release;
                        if (!sRuta.EndsWith("\\")) sRuta += @"\";
                        string sFile = "WP" + obj.Release.Replace(".", "") + string.Format("{0:yyyyMMddhhhhmmss}", DateTime.Now);
                        string dirN1 = ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/VersionOficial/")) + "N+1";
                        string dirFuentes = Path.Combine(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Fuentes")));
                        if (!Directory.Exists(dirFuentes)) Directory.CreateDirectory(dirFuentes);
                        if (ProcessMsg.Version.GenerarInstalador(idVersion, sFile, sRuta, dirN1, dirFuentes) > 0)
                        {
                            string Command = ConfigurationManager.AppSettings["pathGenSetup"];
                            string argument = "\"" + sRuta + sFile + ".iss\"";
                        
                            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo(Command, argument);

                            // Indicamos que la salida del proceso se redireccione en un Stream
                            procStartInfo.RedirectStandardOutput = true;
                            procStartInfo.UseShellExecute = false;

                            //Indica que el proceso no despliegue una pantalla negra (El proceso se ejecuta en background)
                            procStartInfo.CreateNoWindow = false;

                            //Inicializa el proceso
                            System.Diagnostics.Process proc = new System.Diagnostics.Process();
                            proc.StartInfo = procStartInfo;
                            proc.Start();


                            //Consigue la salida de la Consola(Stream) y devuelve una cadena de texto
                            string result = proc.StandardOutput.ReadToEnd();
                            okInstall = true;
                        }
                        if (okInstall)
                        {
                            obj.Estado = 'P';
                            obj.Instalador = sFile + ".exe";
                            return Content(HttpStatusCode.OK, ProcessMsg.Version.UpdVersion(obj.IdVersion, obj));
                        }
                    }
                    return Content(HttpStatusCode.Created, respuesta[1]);
                }
                return Content(HttpStatusCode.Created, obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/CheckInstall/Version/{idVersion:int}/Cliente/{idCliente:int}/Ambiente/{idAmbiente:int}")]
        [HttpGet]
        public Object GetCheckInstall(int idVersion, int idCliente, int idAmbiente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                
                var idVersiones = ProcessMsg.Cliente.GetVersiones(idCliente, null).OrderBy(x => x.IdVersion).Where(x => x.Estado == 'P').Select(x => x.IdVersion);
                if (idVersiones != null)
                {
                    if (idVersion == idVersiones.ElementAt(0))
                    {
                        return true;
                    }
                }
                var obj = ProcessMsg.Version.CheckVersionAnteriorInstalada(idVersion, idCliente, idAmbiente);
                return (obj != null);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/ExisteVersionInicial/Cliente/{idCliente:int}")]
        [HttpGet]
        public Object GetExisteVersionInicial(int idCliente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return ProcessMsg.Cliente.GetVersiones(idCliente, null).Exists(v => v.Release.StartsWith("I"));
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Componentes/{NomComponente}/Comentario")]
        [HttpGet]
        public Object GetComponentesByName(string NomComponente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return ProcessMsg.Componente.GetComponenteByName(NomComponente).OrderByDescending(x => x.idVersion);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        [Route("api/Version/Componentes/{NombreComponente}/Existe")]
        [HttpGet]
        public Object ExisteComponentesModulos(string NombreComponente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                System.IO.FileInfo fi = new System.IO.FileInfo(NombreComponente);

                return fi.Extension.ToUpper().Equals(".SQL") ? true : ProcessMsg.ComponenteModulo.ExisteComponentesModulos(NombreComponente);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/TipoComponentes/{idVersion:int}")]
        [HttpGet]
        public Object GetTipoComponentesByVersion(int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.ComponenteModulo.GetTipoComponentesByVersion(idVersion);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }
                var exist = ProcessMsg.Componente.GetComponentes(idVersion, null).Exists(x => x.Extension.Equals(".sql", StringComparison.OrdinalIgnoreCase));
                if (!exist) obj.RemoveAll(x => x.isCompBD);
                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/ComponentesOficiales")]
        [HttpGet]
        public Object GetComponentesOficiales()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Componente.GetComponentesOficiales(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/VersionOficial/") +"N+1"));
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/UltimaRelease")]
        [HttpGet]
        public Object GetUltimaRelease()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                int vr = 0;
                var lista = ProcessMsg.Version.GetVersiones(null).Where(v => int.TryParse(v.Release.ElementAt(0).ToString(), out vr)).ToList();
                var obj = (lista.Count == 0 ? null : lista.OrderByDescending(x => x.IdVersion).First());
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                return obj.Release;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Cliente/{idCliente:int}/Version/{idVersion:int}/DetalleTarea")]
        [HttpGet]
        public Object GetDetalleTarea(int idCliente, int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Tareas.GetTareas(idCliente, idVersion);
                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Cliente/{idCliente:int}/Version/{idVersion:int}/TareaAtrasada")]
        [HttpGet]
        public Object GetTareaNoEx(int idCliente, int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                bool paso = false;
                var lista = ProcessMsg.Tareas.GetTareas(idCliente, idVersion);
                lista.ForEach(x =>
                {
                    if (x.ExisteAtraso)
                    {
                        paso = true;
                    }
                }
                );

                return paso;
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        // GET: api/Version
        [Route("api/Version")]
        [HttpGet]
        public Object Get()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Version.GetVersiones(null).OrderByDescending(x => x.IdVersion);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}")]
        [HttpGet]
        public Object Get(int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Version.GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Cliente/{idCliente:int}/Componentes")]
        [HttpGet]
        public Object Get(int idVersion, int idCliente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                List<ProcessMsg.Model.AtributosArchivoBo> listaComps = new List<ProcessMsg.Model.AtributosArchivoBo>();
                var obj = ProcessMsg.Version.GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
                var modulosCliente = ProcessMsg.Cliente.GetClientesHasModulo(idCliente);
                obj.Componentes.ForEach(componente =>
                {
                    if (new System.IO.FileInfo(componente.Name).Extension.ToUpper().Equals(".SQL"))
                    {
                        listaComps.Add(componente);
                    }
                    else
                    {
                        modulosCliente.ForEach(modulos =>
                        {
                            if (componente.Modulo.Equals(modulos.NomModulo))
                            {
                                listaComps.Add(componente);
                            }
                        });
                    }
                });
                obj.Componentes = listaComps;

                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Version/{idVersion:int}/Componentes")]
        [HttpGet]
        public Object GetComponentes(int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Componente.GetComponentes(idVersion, null);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Componentes/{nameFile}/script")]
        [HttpGet]
        public Object GetComponentes(int idVersion, string nameFile)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var version = ProcessMsg.Version.GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
                if (version == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                var obj = ProcessMsg.Componente.GetComponentes(version.IdVersion, null).SingleOrDefault(x => x.Name.Equals(nameFile));
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                string dirfmt = string.Format("{0}{1}/{2}", ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Uploads/")), version.Release, obj.Name);

                Byte[] objByte = System.IO.File.ReadAllBytes(dirfmt);
                if (objByte == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ByteArrayContent)null);
                }
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);

                message.Content = new ByteArrayContent(objByte);
                message.Content.Headers.ContentLength = objByte.Length;
                message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                message.Content.Headers.Add("Content-Disposition", "attachment; filename=" +obj.Name.Replace(' ','_'));
                return message;
                
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Componentes/{nameFile}/leerscript")]
        [HttpGet]
        public Object GetContenidoSQL(int idVersion, string nameFile)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var version = ProcessMsg.Version.GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
                if (version == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                var obj = ProcessMsg.Componente.GetComponentes(version.IdVersion, null).SingleOrDefault(x => x.Name.Equals(nameFile));
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                string dirfmt = string.Format("{0}{1}/Scripts/{2}", ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Uploads/")), version.Release, obj.Name);
                var contenidoSQL = System.IO.File.ReadAllText(dirfmt);

                return contenidoSQL;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Tarea/{idCliente:int}/{idAmbiente:int}/{idVersion:int}/{nameFile}/Existe")]
        [HttpGet]
        public Object ExisteTarea(int idCliente, int idAmbiente, int idVersion, string nameFile)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return ProcessMsg.Tareas.ExisteTarea(idCliente, idAmbiente, idVersion, nameFile);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Clientes")]
        [HttpGet]
        public Object GetClientesVersion(int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Cliente.GetClientesVersion(idVersion);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Componentes/{NameFile}/nameFile")]
        [HttpGet]
        public Object GetComponente(int idVersion, string NameFile)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Componente.GetComponenteByName(idVersion, NameFile);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        #endregion

        #region post

        [Route("api/Tareas/{idVersion:int}/{idCliente:int}/{idAmbiente:int}/{codPrf:int}")]
        [HttpPost]
        public Object PostTareas(int idVersion, int idCliente, int idAmbiente, int codPrf)
        {
            try
            {
                List<ProcessMsg.Model.TareaBo> tareas = new List<ProcessMsg.Model.TareaBo>();
                var scripts = ProcessMsg.Componente.GetComponentes(idVersion, null).Where(x => x.Tipo != '*').OrderBy(x => x.Tipo).ToList();
                foreach (var script in scripts)
                {
                    tareas.Add(new ProcessMsg.Model.TareaBo
                    {
                        idClientes = idCliente,
                        Ambientes = new ProcessMsg.Model.AmbienteBo
                        {
                            idAmbientes = idAmbiente
                        },
                        CodPrf = codPrf,
                        Estado = 0,
                        Modulo = script.Modulo,
                        idVersion = idVersion,
                        NameFile = script.Name,
                        Error = ""
                    });
                }

                return Content(HttpStatusCode.OK, ProcessMsg.Tareas.Add(tareas));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/ControlCambios")]
        [HttpPost]
        public Object PostControlCambios([FromBody]ProcessMsg.Model.ControlCambiosBo control)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var usuario = ProcessMsg.Seguridad.GetUsuario(int.Parse(HttpContext.Current.Session["token"].ToString()));
                if (usuario != null)
                {
                    control.Usuario = usuario.Persona.Nombres + " " + usuario.Persona.Apellidos;
                    control.Fecha = DateTime.Now;
                    var pathVoN1 = ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/VersionOficial/"));
                    pathVoN1 = Path.Combine(pathVoN1, "N+1");
                    if (ProcessMsg.Version.AddControlCambios(control)) {
                        control.VersionFmt = ProcessMsg.Version.GetVersion(control.Version);
                        ProcessMsg.Bitacora.AddBitacora("Version", "", control.Bitacora('I'), 'I', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), control.Bitacora('?'));
                        return Content(HttpStatusCode.OK, true);
                    }
                }
                return Content(HttpStatusCode.Created, (object)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Cliente/{idClientes:int}/Version/{idVersion:int}/ReportarTodasTareas")]
        [HttpPost]
        public Object PostReportTodasTareas(int idClientes, int idVersion, [FromBody]List<ProcessMsg.Model.TareaBo> tareas)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                string msg = string.Format("Reporte de Tareas Atrasadas en el cliente {0}"
                    , idClientes);

                tareas.ForEach(x => 
                {
                    if (x.Estado == 0 || x.Estado == 2 || x.Estado == 4)
                    {
                        msg += string.Format("<br><br>Ambiente: {0}<br>Estado: {1}<br>ID Versión: {2}<br>Fecha y Hora de Registro: {3}<br>Archivo: {4}<br>ERROR: {5}"
                       , x.Ambientes.Nombre, x.EstadoFmt, x.idVersion, x.FechaRegistroFmt, x.NameFile, x.Error);
                    }
                    
                }
                );


                var res = ProcessMsg.Utils.SendMail(msg, "Reporte Tarea Atrasada", ProcessMsg.Utils.CorreoSoporte);
                if (res)
                {
                    if (ProcessMsg.Tareas.ReportarTodasTareas(idClientes, idVersion) > 0)
                    {
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }


                return Content(response.StatusCode, res);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/ReportarTareaAtrasada")]
        [HttpPost]
        public Object PostReportTareaAtrasada([FromBody]ProcessMsg.Model.TareaBo tarea)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                string msg = string.Format("Reporte de Tareas Atrasadas en el cliente {0}<br><br>Ambiente: {1}<br>Estado: {2}<br>ID Versión: {3}<br>Fecha y Hora de Registro: {4}<br>Archivo: {5}<br>ERROR: {6}"
                    , tarea.idClientes, tarea.Ambientes.Nombre, tarea.EstadoFmt, tarea.idVersion, tarea.FechaRegistroFmt, tarea.NameFile, tarea.Error);

                var res = ProcessMsg.Utils.SendMail(msg, "Reporte Tarea Atrasada", ProcessMsg.Utils.CorreoSoporte);
                if (res)
                {
                    if (ProcessMsg.Tareas.ReportarTarea(tarea.idTareas) > 0)
                    {
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }


                return Content(response.StatusCode, res);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Version")]
        [HttpPost]
        public Object Post([FromBody]ProcessMsg.Model.VersionBo version)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (version.Release == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (version.Fecha.Year == 1)
                    version.Fecha = DateTime.Now;
                ; if (version.Estado.ToString() == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    if (version.Release.Equals("I")) version.Release = string.Format("I{0:dd.MM.yyyy.HH.mm.ss}",DateTime.Now);

                    var obj = ProcessMsg.Version.AddVersion(version);
                    if (obj == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        ProcessMsg.Bitacora.AddBitacora("Version", "", obj.Bitacora('I'), 'I', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), obj.Bitacora('?'));
                        return Content(HttpStatusCode.Created, obj);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.VersionBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/VersionInicial/{idVersion:int}")]
        [HttpPost]
        public Object PostVersionInicial(int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Version.GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
                if (obj != null)
                {
                    var upload = ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Uploads"));
                    var incial = Path.Combine(upload, obj.Release);
                    if (!Directory.Exists(incial))
                    {
                        Directory.CreateDirectory(incial);
                    }
                    else
                    {
                        Directory.Delete(incial, true);
                        Directory.CreateDirectory(incial);
                    }
                    var mods = ProcessMsg.Modulo.GetModulos(null);
                    var comps = new List<ProcessMsg.Model.AtributosArchivoBo>();
                    var CompModulo = ProcessMsg.ComponenteModulo.GetComponentesConDirectorio();
                    DirectoryInfo di = new DirectoryInfo(Path.Combine(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/VersionOficial/")), "N+1"));
                    di.GetDirectories().ToList().ForEach(dir =>
                    {
                        dir.GetFiles().ToList().ForEach(file =>
                        {

                            if ((file.Attributes & FileAttributes.System) != FileAttributes.System)
                            {
                                foreach (var c in CompModulo)
                                {
                                    if (c.Nombre.Equals(file.Name) 
                                    && mods.Exists(x => x.idModulo == c.Modulo && x.isCore)
                                    && file.DirectoryName.EndsWith(c.Directorio) 
                                    && !comps.Exists(x => x.Name.ToUpper().Equals(file.Name.ToUpper())))
                                    {
                                        file.CopyTo(Path.Combine(incial, file.Name), true);
                                        comps.Add(new ProcessMsg.Model.AtributosArchivoBo
                                        {
                                            Name = file.Name,
                                            Modulo = c.NomModulo,
                                            DateCreate = file.CreationTime,
                                            LastWrite = file.LastWriteTime
                                        });
                                    }
                                }
                            }
                        });
                    });
                    var respuesta = ProcessMsg.Componente.AddComponentes(obj.IdVersion, comps);
                    if (respuesta[0].Equals("0"))
                    {
                        return Content(HttpStatusCode.OK, obj);
                    }
                    return Content(HttpStatusCode.Created, respuesta[1]);
                }
                return Content(HttpStatusCode.Created, obj);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Version/{idVersion:int}/Componentes")]
        [HttpPost]
        public Object PostComponentes(int idVersion, [FromBody]ProcessMsg.Model.AtributosArchivoBo archivo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (archivo == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (archivo.Name == null || archivo.Modulo == null || archivo.LastWrite == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var existe = ProcessMsg.Componente.GetComponentes(archivo.idVersion, null).SingleOrDefault(x => x.Modulo.Equals(archivo.Modulo) && x.Name.Equals(archivo.Name));
                    if (existe == null)
                    {
                        if (ProcessMsg.Componente.AddComponente(idVersion, archivo) == 1)
                        {
                            ProcessMsg.Bitacora.AddBitacora("Version", "", archivo.Bitacora('I'), 'I', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), archivo.Bitacora('?'));
                        }
                    }
                    return Content(HttpStatusCode.Created, true);

                }

                return Content(response.StatusCode, (ProcessMsg.Model.AtributosArchivoBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/LComponentes")]
        [HttpPost]
        public Object PostListaComponentes(int idVersion, [FromBody]IEnumerable<ProcessMsg.Model.AtributosArchivoBo> archivo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                return Content(response.StatusCode, (ProcessMsg.Model.AtributosArchivoBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Clientes/TipoPub/{TipoPub:int}")]
        [HttpPost]
        public Object PostClientesToVersion(int idVersion, [FromBody] List<ClienteToVersion> listaClientes, int TipoPub)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var idClientes = TipoPub == 1 ? listaClientes.Where(y => y.check).Select(x => x.cliente.Id).ToList() : listaClientes.Select(x => x.cliente.Id).ToList();
                var res = ProcessMsg.Version.AddVersionCliente(idVersion, idClientes);
                if (res[0].ToString().Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    return Content(HttpStatusCode.OK, res[1]);
                }
                return Content(HttpStatusCode.Created, res[1]);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Cliente/{idCliente:int}")]
        [HttpPost]
        public Object PostVersionCliente(int idVersion, int idCliente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (idVersion <= 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (idCliente <= 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var retorno = ProcessMsg.Version.AddVersionCliente(idVersion, idCliente);
                    if (retorno == 0)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        return Content(HttpStatusCode.Created, (ProcessMsg.Model.VersionBo)null);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.VersionBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Cliente/{idCliente:int}/Ambiente")]
        [HttpPost]
        public Object PostVersionAmbienteCliente(int idVersion, int idCliente, [FromBody]ProcessMsg.Model.AmbienteBo ambiente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (idVersion <= 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (idCliente <= 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var retorno = ProcessMsg.Version.AddVersionAmbiente(idVersion, ambiente.idAmbientes, idCliente, ambiente.Estado);
                    if (retorno == 0)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        return Content(HttpStatusCode.Created, (ProcessMsg.Model.AmbienteBo)null);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.AmbienteBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Tarea")]
        [HttpPost]
        public Object PostTarea(int idVersion, [FromBody]ProcessMsg.Model.TareaBo tarea)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                tarea.idVersion = idVersion;
                int res = ProcessMsg.Tareas.Add(tarea);
                if (res == 1)
                {
                    response.StatusCode = HttpStatusCode.OK;
                }

                return Content(response.StatusCode, res);
            }catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        #endregion

        #region put
        [Route("api/ControlCambios")]
        [HttpPut]
        public Object PutControlCambios([FromBody]ProcessMsg.Model.ControlCambiosBo control)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var usuario = ProcessMsg.Seguridad.GetUsuario(int.Parse(HttpContext.Current.Session["token"].ToString()));
                if (usuario != null)
                {
                    var cc = ProcessMsg.Version.GetControlCambios(control.Version).SingleOrDefault(x => x.Tips == control.Tips && x.Modulo == control.Modulo);
                    if (cc != null)
                    {
                        control.Usuario = usuario.Persona.Nombres + " " + usuario.Persona.Apellidos;
                        control.Fecha = DateTime.Now;
                        if (ProcessMsg.Version.UpdControlCambios(control))
                        {
                            control.VersionFmt = cc.VersionFmt;
                            ProcessMsg.Bitacora.AddBitacora("Version", cc.Bitacora('U'), cc.Bitacora('U'), 'U', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), control.Bitacora('?'));
                            return Content(HttpStatusCode.OK, ProcessMsg.Version.GetControlCambios(control.Version).SingleOrDefault(x => x.Tips == control.Tips && x.Modulo == control.Modulo));
                        }
                    }
                }
                return Content(HttpStatusCode.Created, (object)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Estado/{Estado}/Upd")]
        [HttpPut]
        public Object UpdEstadoVersion(int idVersion, char Estado)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var version = ProcessMsg.Version.GetVersiones(null).SingleOrDefault(v => v.IdVersion == idVersion);
                if (version == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }
                version.Estado = Estado;
                ProcessMsg.Bitacora.AddBitacora("Version", "", "", Estado == 'C' ? 'N' : 'V', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), version.Bitacora('?'));
                return Content(HttpStatusCode.OK, ProcessMsg.Version.UpdEstadoVersion(idVersion, version));
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/ReportarTareaManual")]
        [HttpPut]
        public Object ReportarTareaManual([FromBody]ProcessMsg.Model.TareaBo tarea)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                var obj = ProcessMsg.Tareas.SetEstadoTarea(tarea.idTareas, tarea.Estado, tarea.Error);
                if (obj == 0)
                {
                    response.StatusCode = HttpStatusCode.Accepted;
                }
                else
                {
                    return Content(HttpStatusCode.OK, obj);
                }

                return Content(response.StatusCode, (ProcessMsg.Model.TareaBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}")]
        [HttpPut]
        public Object Put(int idVersion, [FromBody]ProcessMsg.Model.VersionBo version)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (version.Release == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (version.Fecha == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var versionAnt = ProcessMsg.Version.GetVersion(idVersion);
                    var obj = ProcessMsg.Version.UpdVersion(idVersion, version);
                    if (obj == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        ProcessMsg.Bitacora.AddBitacora("Version", versionAnt.Bitacora('U'), obj.Bitacora('U'), 'U', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), obj.Bitacora('?'));
                        return Content(HttpStatusCode.Created, obj);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.VersionBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Componentes")]
        [HttpPut]
        public Object PutComponentes(int idVersion, [FromBody]ProcessMsg.Model.AtributosArchivoBo archivo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (archivo == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (archivo.Name == null || archivo.Modulo == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var obj = ProcessMsg.Componente.UpdComponente(idVersion, archivo);
                    if (obj == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        return Content(HttpStatusCode.Created, obj);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.AtributosArchivoBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        #endregion

        #region delete
        [Route("api/Version/{idVersion:int}")]
        [HttpDelete]
        public Object DeleteVersion(int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
                var dirUpload = ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Uploads"));
                var version = ProcessMsg.Version.GetVersion(idVersion);
                if (version != null)
                {
                    if (ProcessMsg.Version.DelVersion(idVersion) <= 0)
                    {
                        ProcessMsg.Bitacora.AddBitacora("Version",version.Bitacora('D'),"",'D',DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), version.Bitacora('?'));
                        Directory.Delete(Path.Combine(dirUpload,version.Release), true);
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                }
                

                return Content(response.StatusCode, (ProcessMsg.Model.AtributosArchivoBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Componentes")]
        [HttpDelete]
        public Object DeleteComponentes(int idVersion, [FromBody]ProcessMsg.Model.AtributosArchivoBo archivo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (archivo == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (archivo.Name == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    if (ProcessMsg.Componente.DelComponente(idVersion, archivo) <= 0)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    ProcessMsg.Bitacora.AddBitacora("Version", archivo.Bitacora('D'), "", 'D', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), archivo.Bitacora('?'));
                }

                return Content(response.StatusCode, (ProcessMsg.Model.AtributosArchivoBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/delDocCambios/{idVersion:int}/{tips:int}/{modulo:int}")]
        [HttpDelete]
        public Object DelDocCambios(int idVersion, int tips, int modulo, string doc)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var dirFuente = ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Fuentes"));
                if (ProcessMsg.Version.DelDocCambios(dirFuente, doc, idVersion, tips, modulo) == 1)
                {
                    return Content(HttpStatusCode.OK, ProcessMsg.Version.GetControlCambios(idVersion).SingleOrDefault(x => x.Modulo == modulo && x.Tips == tips));
                }
                return Content(HttpStatusCode.Created, (object)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        [Route("api/delControlCambios/{Version:int}/{Tips:int}/{Modulo:int}")]
        [HttpDelete]
        public Object DelControlCambios(int Version, int Tips, int Modulo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var cc = ProcessMsg.Version.GetControlCambios(Version).SingleOrDefault(x => x.Tips == Tips && x.Modulo == Modulo);
                if (cc != null)
                {
                    ProcessMsg.Bitacora.AddBitacora("Version", cc.Bitacora('D'), "", 'D', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), cc.Bitacora('?'));
                    var dirFuente = ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Fuentes"));
                    return Content(HttpStatusCode.OK, ProcessMsg.Version.DelControlCambios(dirFuente, Version, Tips, Modulo));
                }
                return Content(HttpStatusCode.BadRequest, (object)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        #endregion
    }
}
