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
        #region get
        [Route("api/Componentes/Sync")]
        [HttpGet]
        public Object SyncComponentes()
        {
            try
            {
                string log = "OK.\n\n";
                bool hasTipoNull = false;
                List<ProcessMsg.Model.ComponenteModuloBo> componentesModulos = new List<ProcessMsg.Model.ComponenteModuloBo>();
                var Modulos = ProcessMsg.Modulo.GetModulos(null);
                var TipoComponentes = ProcessMsg.ComponenteModulo.GetTipoComponentes();
                foreach (var m in Modulos)
                {
                    DirectoryInfo di = new DirectoryInfo(Path.Combine(HttpContext.Current.Server.MapPath("~/VersionOficial/N+1"), @m.Directorio));
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
                        log += "\nComponentes sincronizados con exito!";
                        return log;
                    }
                    log += "\nERROR SQL: " + r[1] + "(" + r[0] + ")";
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
                return ProcessMsg.ComponenteModulo.GetComponentesModulos(idModulo);
            }
            catch(Exception ex)
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
                string dirfmt = string.Format("{0}", HttpContext.Current.Server.MapPath("~/Fuentes/PlanillaModulos.xlsx"));

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

        #endregion

        #region post

        [Route("api/TipoComponentes")]
        [HttpPost]
        public Object PostTipoComponentes([FromBody]ProcessMsg.Model.TipoComponenteBo TipoComponente)
        {
            try
            {
                if (ProcessMsg.ComponenteModulo.AddTipoComponentes(TipoComponente.Nombre,TipoComponente.isCompBD, TipoComponente.isCompDLL, TipoComponente.Extensiones) == 1)
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
                if (ProcessMsg.ComponenteModulo.AddComponentesModulos(ComponenteModulo.Nombre,ComponenteModulo.Descripcion,idModulo,ComponenteModulo.TipoComponentes.idTipoComponentes) == 1)
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

        [Route("api/Modulo")]
        [HttpPost]
        public Object PostModulo([FromBody] ProcessMsg.Model.ModuloBo modulo)
        {
            try
            {
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
                if (ProcessMsg.ComponenteModulo.UpdTipoComponentes(idTipoComponentes, TipoComponente.Nombre, TipoComponente.Extensiones,TipoComponente.isCompBD,TipoComponente.isCompDLL) == 1)
                {
                    return Content(HttpStatusCode.OK, true);
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
                if (ProcessMsg.ComponenteModulo.UpdComponentesModulos(idComponentesModulos,ComponenteModulo.Nombre,ComponenteModulo.Descripcion,ComponenteModulo.TipoComponentes.idTipoComponentes) == 1)
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

        [Route("api/Modulo/{idModulo:int}/Vigente")]
        [HttpPut]
        public Object SetVigente(int idModulo)
        {
            try
            {
                if (ProcessMsg.Modulo.SetVigente(idModulo) == 1)
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

        [Route("api/Modulo/{idModulo:int}")]
        [HttpPut]
        public Object UpdModulo(int idModulo, [FromBody] ProcessMsg.Model.ModuloBo modulo)
        {
            try
            {
                modulo.idModulo = idModulo;
                if (ProcessMsg.Modulo.UpdModulo(modulo) == 1)
                {
                    return Content(HttpStatusCode.OK, true);
                }
                return Content(HttpStatusCode.Created, false);
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
                if (ProcessMsg.ComponenteModulo.DelComponentesModulos(idComponentesModulos) == 1)
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

        [Route("api/TipoComponentes/{idTipoComponentes:int}")]
        [HttpDelete]
        public Object DelTipoComponentes(int idTipoComponentes)
        {
            try
            {
                if (ProcessMsg.ComponenteModulo.DelTipoComponentes(idTipoComponentes) == 1)
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

        [Route("api/Modulo/{idModulo:int}")]
        [HttpDelete]
        public Object DelModulo(int idModulo)
        {
            try
            {
                if (ProcessMsg.Modulo.DelModulo(idModulo) == 1)
                {
                    return Content(HttpStatusCode.OK, true);
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
