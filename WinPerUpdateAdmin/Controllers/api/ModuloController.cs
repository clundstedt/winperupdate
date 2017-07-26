using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class ModuloController : ApiController
    {
        #region Clases
        public class ComponenteDir
        {
            public bool check { get; set; }
            public ProcessMsg.Model.ComponenteModuloBo componente { get; set; }
        }
        public class DirModulos
        {
            public string Directorio { get; set; }
        }
        #endregion

        #region get

        [Route("api/isModuloVigente")]
        [HttpGet]
        public Object isModuloVigente(string fileName)
        {
            try
            {
                var mods = ProcessMsg.Modulo.GetModulosByComponente(fileName);

                return Content(HttpStatusCode.OK, mods.Where(x=> x.Estado == 'V').ToList().Count > 0);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        [Route("api/getModulosDesdeSuite/{Suite:int}")]
        [HttpGet]
        public Object GetModulosDesdeSuite(int Suite)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var suite = ProcessMsg.Suites.GetSuites().SingleOrDefault(x => x.idSuite == Suite);
                if (suite == null)
                {
                    return Content(HttpStatusCode.BadRequest, (object)null);
                }
                return Content(HttpStatusCode.OK, ProcessMsg.Modulo.GetModulosBySuites(suite.Subsuites).Where(x=> x.Estado =='V'));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        [Route("api/getModulosVersion/{idVersion:int}")]
        [HttpGet]
        public Object GetModulosVersion(int idVersion)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var nombresMods = ProcessMsg.Version.GetModulosVersiones(idVersion, null).Distinct().ToList();
                var mod = ProcessMsg.Modulo.GetModulos(null).Where(x => nombresMods.Exists(y => y.Equals(x.NomModulo, StringComparison.OrdinalIgnoreCase)));

                return Content(HttpStatusCode.OK, mod.OrderBy(x => x.NomModulo));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        /// <summary>
        /// Retorna la lista de componentes actuales que existen en el directorio del modulo especificado.
        /// </summary>
        /// <param name="idModulo"></param>
        /// <returns></returns>
        [Route("api/Modulo/{idModulo:int}/ComponentesDir")]
        [HttpGet]
        public Object GetComponentesDirectorio(int idModulo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                List<ProcessMsg.Model.ComponenteModuloBo> lista = new List<ProcessMsg.Model.ComponenteModuloBo>();
                var tipos = ProcessMsg.ComponenteModulo.GetTipoComponentes();
                var mod = ProcessMsg.Modulo.GetModulo(idModulo);
                if (mod != null)
                {
                    var compsDir = new DirectoryInfo(Path.Combine(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/VersionOficial")), "N+1", mod.Directorio)).GetFiles().ToList();
                    var comps = ProcessMsg.ComponenteModulo.GetComponentesModulos(idModulo);
                    compsDir.ForEach(cd =>
                    {
                        if ((cd.Attributes & FileAttributes.System) != FileAttributes.System)
                        {
                            if (!comps.Exists(x => x.Nombre.Equals(cd.Name, StringComparison.OrdinalIgnoreCase)))
                            {
                                lista.Add(new ProcessMsg.Model.ComponenteModuloBo
                                {
                                    Nombre = cd.Name,
                                    TipoComponentes = tipos.SingleOrDefault(x => x.Extensiones.Contains(cd.Extension)),
                                    Modulo = idModulo
                                });
                            }
                        }
                    });
                    return Content(HttpStatusCode.OK, lista);
                }
                return Content(HttpStatusCode.Accepted, (ProcessMsg.Model.ComponenteModuloBo)null);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Modulos/Suite/{idSuite:int}")]
        [HttpGet]
        public Object GetModulosBySuite(int idSuite)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return ProcessMsg.Modulo.GetModulosBySuite(idSuite).Where(m => m.Estado == 'V').OrderBy(m => m.NomModulo);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Componentes/Sync")]
        [HttpGet]
        public Object SyncComponentes()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                string log = "OK.\n\n";
                if (ProcessMsg.ComponenteModulo.DelAll())
                {
                    bool hasTipoNull = false;
                    List<ProcessMsg.Model.ComponenteModuloBo> componentesModulos = new List<ProcessMsg.Model.ComponenteModuloBo>();
                    var Modulos = ProcessMsg.Modulo.GetModulos(null);
                    var TipoComponentes = ProcessMsg.ComponenteModulo.GetTipoComponentes();
                    foreach (var m in Modulos)
                    {
                        DirectoryInfo di = new DirectoryInfo(Path.Combine(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/VersionOficial/")), "N+1", @m.Directorio));
                        di.GetFiles().ToList().ForEach(file =>
                        {

                            if ((file.Attributes & FileAttributes.System) != FileAttributes.System)
                            {
                                componentesModulos.Add(new ProcessMsg.Model.ComponenteModuloBo
                                {
                                    Modulo = m.idModulo,
                                    Nombre = file.Name,
                                    TipoComponentes = TipoComponentes.Find(x => x.Extensiones.Contains(file.Extension.ToLower()))
                                });
                            }
                        });
                        di.GetDirectories().ToList().ForEach(dir =>
                        {
                            log += string.Format("{0} es un directorio.\n", dir.Name);
                        });
                        componentesModulos.Where(c => c.TipoComponentes == null).ToList().ForEach(c =>
                        {
                            hasTipoNull = true;
                            log += string.Format("{0} No contiene un tipo de componente valido.\n", c.Nombre);
                        });

                    }
                    if (!hasTipoNull)
                    {
                        var r = ProcessMsg.ComponenteModulo.AddComponentesModulos(componentesModulos);
                        if (int.Parse(r[0].ToString()) == 0)
                        {
                            log += "\nComponentes sincronizados con éxito!";
                            return log;
                        }
                        log += "\nERROR SQL: " + r[1] + "(" + r[0] + ")";
                    }
                    else
                    {
                        log += "Sincronización fallida, existen componentes cuyo tipo no existe.";
                    }
                }
                else
                {
                    log += "ERROR: No se pudieron eliminar los componentes actuales.";
                }
                return log;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/TipoComponentes")]
        [HttpGet]
        public Object GetTipoComponentes()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return ProcessMsg.ComponenteModulo.GetTipoComponentes();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Modulo/{idModulo:int}/Componentes")]
        [HttpGet]
        public Object GetComponentesModulos(int idModulo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return ProcessMsg.ComponenteModulo.GetComponentesModulos(idModulo);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        [Route("api/Modulo/{idModulo:int}/{Tipo:int}/Componente")]
        [HttpGet]
        public Object GetComponenteModulo(int idModulo, int Tipo, string Comp)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var compMod = ProcessMsg.ComponenteModulo.GetComponentesModulos(idModulo).Exists(x => x.Nombre.Equals(Comp));

                var tipoComp = ProcessMsg.ComponenteModulo.GetTipoComponentes().SingleOrDefault(x => x.idTipoComponentes == Tipo);
                var eq = tipoComp.Extensiones.Contains(new FileInfo(Comp).Extension);

                if (!compMod && eq)
                {
                    return Content(HttpStatusCode.OK, true);
                }

                return Content(HttpStatusCode.OK, false);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        [Route("api/Modulo/{idModulo:int}")]
        [HttpGet]
        public Object GetModulo(int idModulo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Modulo.GetModulo(idModulo);
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

        [Route("api/Modulos")]
        [HttpGet]
        public Object Get()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Modulo.GetModulos(null);
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
        
        [Route("api/ModulosXlsx/{idUsuario:int}")]
        [HttpGet]
        public Object GetModulosXlsx(int idUsuario)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return ProcessMsg.Modulo.GetModulosXlsx(idUsuario);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Modulos/Planilla")]
        [HttpGet]
        public Object GetPlanillaModulosXLSX()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                string dirfmt = string.Format("{0}", ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Fuentes/")) + "PlanillaModulos.xlsx");

                Byte[] objByte = System.IO.File.ReadAllBytes(dirfmt);
                if (objByte == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ByteArrayContent)null);
                }
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Created);

                message.Content = new ByteArrayContent(objByte);
                message.Content.Headers.ContentLength = objByte.Length;
                message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                message.Content.Headers.Add("Content-Disposition", "attachment; filename=PlanillaModulos.xlsx");
                message.StatusCode = HttpStatusCode.OK;

                return message;

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        [Route("api/ExisteComponenteEnDir")]
        [HttpGet]
        public Object ExisteComponenteEnDir(string nombreComp, string dir)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var path = Path.Combine(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/VersionOficial")), "N+1", dir.Trim(), nombreComp.Trim());
                var exist = File.Exists(path);
                return Content(HttpStatusCode.OK, exist);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        #endregion

        #region post
        [Route("api/getModulosByComponente")]
        [HttpPost]
        public Object getModulosByComponente([FromBody]DirModulos filename)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return Content(HttpStatusCode.OK, ProcessMsg.Modulo.GetModulosByComponente(filename.Directorio));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        [Route("api/ExistDir/Modulo")]
        [HttpPost]
        public Object GetExisteDirModulo([FromBody]DirModulos dm)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                if (dm == null)
                {
                    return false;
                }
                var path = Path.Combine(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/VersionOficial")), "N+1", dm.Directorio);
                return Directory.Exists(path);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        [Route("api/getDir")]
        [HttpPost]
        public Object getDir([FromBody]DirModulos dm)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                
                var dirVo = Path.Combine(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/VersionOficial")));
                if (dm.Directorio.StartsWith(dirVo))
                {
                    //dm.Directorio = dm.Directorio.Replace(dirVo, "");
                }
                var path = Path.Combine(dirVo, dm.Directorio);
                if (Directory.Exists(path))
                {
                    return Content(HttpStatusCode.OK, new DirectoryInfo(path).GetDirectories());
                }
                return Content(HttpStatusCode.BadRequest, (object)path);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Modulo/ComponentesDir/{idModulo:int}")]
        [HttpPost]
        public Object PostComponentesDir(int idModulo, [FromBody] List<ComponenteDir> ComponentesDir)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var comps = ComponentesDir.Where(x => x.check).Select(x => x.componente).ToList();
                if (comps != null)
                {
                    var mod = ProcessMsg.Modulo.GetModulo(idModulo);
                    if (mod != null)
                    {
                        List<ProcessMsg.Model.BitacoraBo> bitacoras = new List<ProcessMsg.Model.BitacoraBo>();
                        foreach (var c in comps)
                        {
                            c.NomModulo = mod.NomModulo;
                            bitacoras.Add(new ProcessMsg.Model.BitacoraBo
                            {
                                Menu = "Modulo",
                                Vant = "",
                                Vnue = c.Bitacora('I'),
                                Accion = 'I',
                                Fecha = DateTime.Now,
                                Usuario = int.Parse(HttpContext.Current.Session["token"].ToString()),
                                Registro = c.Bitacora('?')
                            });
                        }
                        ProcessMsg.Bitacora.AddBitacora(bitacoras);
                        var res = ProcessMsg.ComponenteModulo.AddComponentesModulos(comps);
                        if (res[0].ToString().Equals("0"))
                        {
                            return Content(HttpStatusCode.OK, res[1]);
                        }

                        return Content(HttpStatusCode.Created, res[1]);
                    }
                }
                return Content(HttpStatusCode.Accepted, "Error: ComponentesDir NULL");
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/TipoComponentes")]
        [HttpPost]
        public Object PostTipoComponentes([FromBody]ProcessMsg.Model.TipoComponenteBo TipoComponente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                ProcessMsg.Bitacora.AddBitacora("Modulo", null, TipoComponente.Bitacora('I'), 'I', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), TipoComponente.Bitacora('?'));
                if (ProcessMsg.ComponenteModulo.AddTipoComponentes(TipoComponente.Nombre,TipoComponente.isCompBD, TipoComponente.isCompDLL, TipoComponente.Extensiones, TipoComponente.isCompCambios) == 1)
                {
                    return Content(HttpStatusCode.OK, true);
                }
                return Content(HttpStatusCode.Created, false);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/ComponentesModulos/{idModulo:int}")]
        [HttpPost]
        public Object PostComponentesModulos(int idModulo, [FromBody] ProcessMsg.Model.ComponenteModuloBo ComponenteModulo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var mod = ProcessMsg.Modulo.GetModulo(idModulo);
                if (mod != null)
                {
                    ComponenteModulo.NomModulo = mod.NomModulo;
                    ProcessMsg.Bitacora.AddBitacora("Modulo", null, ComponenteModulo.Bitacora('I'), 'I', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), ComponenteModulo.Bitacora('?'));

                    if (ProcessMsg.ComponenteModulo.AddComponentesModulos(ComponenteModulo.Nombre, ComponenteModulo.Descripcion, idModulo, ComponenteModulo.TipoComponentes.idTipoComponentes) == 1)
                    {
                        return Content(HttpStatusCode.OK, true);
                    }
                }
                return Content(HttpStatusCode.Created, false);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Modulo")]
        [HttpPost]
        public Object PostModulo([FromBody] ProcessMsg.Model.ModuloBo modulo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                ProcessMsg.Bitacora.AddBitacora("Modulo", null, modulo.Bitacora('I'), 'I', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), modulo.Bitacora('?'));


                var moduloRes = ProcessMsg.Modulo.AddModulo(modulo);
                if (moduloRes != null)
                {
                    return Content(HttpStatusCode.OK, (ProcessMsg.Model.ModuloBo)moduloRes);
                }
                return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.ModuloBo)null);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        #endregion

        #region put
        [Route("api/TipoComponentes/{idTipoComponentes:int}")]
        [HttpPut]
        public Object UpdComponentesModulos(int idTipoComponentes, [FromBody]ProcessMsg.Model.TipoComponenteBo TipoComponente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var tipo = ProcessMsg.ComponenteModulo.GetTipoComponentes().SingleOrDefault(x => x.idTipoComponentes == idTipoComponentes);
                if (tipo != null)
                {
                    ProcessMsg.Bitacora.AddBitacora("Modulo", tipo.Bitacora('U'), TipoComponente.Bitacora('U'), 'U', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), tipo.Bitacora('?'));
                    if (ProcessMsg.ComponenteModulo.UpdTipoComponentes(idTipoComponentes, TipoComponente.Nombre, TipoComponente.Extensiones, TipoComponente.isCompBD, TipoComponente.isCompDLL, TipoComponente.isCompCambios) == 1)
                    {
                        return Content(HttpStatusCode.OK, true);
                    }
                }
                return Content(HttpStatusCode.Created, false);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/ComponentesModulos/{idComponentesModulos:int}")]
        [HttpPut]
        public Object UpdComponentesModulos(int idComponentesModulos,[FromBody]ProcessMsg.Model.ComponenteModuloBo ComponenteModulo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var comp = ProcessMsg.ComponenteModulo.GetComponentesConDirectorio(idComponentesModulos);
                if (comp != null)
                {
                    ProcessMsg.Bitacora.AddBitacora("Modulo", comp.Bitacora('U'), ComponenteModulo.Bitacora('U'), 'U', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), comp.Bitacora('?'));

                    if (ProcessMsg.ComponenteModulo.UpdComponentesModulos(idComponentesModulos, ComponenteModulo.Nombre, ComponenteModulo.Descripcion, ComponenteModulo.TipoComponentes.idTipoComponentes) == 1)
                    {
                        return Content(HttpStatusCode.OK, true);
                    }
                }
                return Content(HttpStatusCode.Created, false);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Modulo/{idModulo:int}/Vigente")]
        [HttpPut]
        public Object SetVigente(int idModulo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var mod = ProcessMsg.Modulo.GetModulo(idModulo);
                if (mod != null)
                {
                    ProcessMsg.Bitacora.AddBitacora("Modulo", null, null, 'V', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), mod.Bitacora('?'));
                    if (ProcessMsg.Modulo.SetVigente(idModulo) == 1)
                    {
                        return Content(HttpStatusCode.OK, true);
                    }
                }
                
                return Content(HttpStatusCode.Created, false);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Modulo/{idModulo:int}")]
        [HttpPut]
        public Object UpdModulo(int idModulo, [FromBody] ProcessMsg.Model.ModuloBo modulo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                modulo.idModulo = idModulo;
                var modAnt = ProcessMsg.Modulo.GetModulo(idModulo);
                if (modAnt != null)
                {
                    ProcessMsg.Bitacora.AddBitacora("Modulo", modAnt.Bitacora('U'), modulo.Bitacora('U'), 'U', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), modulo.Bitacora('?'));

                    if (ProcessMsg.Modulo.UpdModulo(modulo) == 1)
                    {
                        return Content(HttpStatusCode.OK, modulo);
                    }
                }
                return Content(HttpStatusCode.Created, (object) null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        #endregion

        #region delete
        [Route("api/ComponentesModulos/{idComponentesModulos:int}")]
        [HttpDelete]
        public Object DelComponentesModulos(int idComponentesModulos)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var comp = ProcessMsg.ComponenteModulo.GetComponentesConDirectorio(idComponentesModulos);
                if (comp != null)
                {
                    ProcessMsg.Bitacora.AddBitacora("Modulo", comp.Bitacora('D'), null, 'D', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), comp.Bitacora('?'));
                    if (ProcessMsg.ComponenteModulo.DelComponentesModulos(idComponentesModulos) == 1)
                    {
                        return Content(HttpStatusCode.OK, true);
                    }
                }
                return Content(HttpStatusCode.Created, false);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/TipoComponentes/{idTipoComponentes:int}")]
        [HttpDelete]
        public Object DelTipoComponentes(int idTipoComponentes)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var tipo = ProcessMsg.ComponenteModulo.GetTipoComponentes().SingleOrDefault(x => x.idTipoComponentes == idTipoComponentes);
                if (tipo != null)
                {
                    ProcessMsg.Bitacora.AddBitacora("Modulo", tipo.Bitacora('D'), null, 'D', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), tipo.Bitacora('?'));
                    if (ProcessMsg.ComponenteModulo.DelTipoComponentes(idTipoComponentes) == 1)
                    {
                        return Content(HttpStatusCode.OK, true);
                    }
                }
                return Content(HttpStatusCode.Created, false);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Modulo/{idModulo:int}")]
        [HttpDelete]
        public Object DelModulo(int idModulo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var mod = ProcessMsg.Modulo.GetModulo(idModulo);
                if (mod != null)
                {
                    ProcessMsg.Bitacora.AddBitacora("Modulo", null, null, 'N', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), mod.Bitacora('?'));

                    if (ProcessMsg.Modulo.DelModulo(idModulo) == 1)
                    {
                        return Content(HttpStatusCode.OK, true);
                    }
                }
                return Content(HttpStatusCode.Created, false);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        #endregion
    }
}
