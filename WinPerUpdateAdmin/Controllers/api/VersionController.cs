using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class VersionController : ApiController
    {
        #region get
        [Route("api/Version/NuevoRelease")]
        [HttpGet]
        public Object GetNuevoRelease()
        {
            try
            {
                var obj = ProcessMsg.Version.GetVersiones(null).OrderByDescending(x => x.IdVersion).ElementAt(0);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                return obj != null ? ProcessMsg.Utils.GenerarVersionSiguiente(obj.Release) : "";
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

        [Route("api/Version/{idVersion:int}/Componentes")]
        [HttpGet]
        public Object GetComponentes(int idVersion)
        {
            try
            {
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

                string dirfmt = string.Format("{0}{1}/{2}", HttpContext.Current.Server.MapPath("~/Uploads/"), version.Release, obj.Name);

                Byte[] objByte = System.IO.File.ReadAllBytes(dirfmt);
                if (objByte == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ByteArrayContent)null);
                }
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);

                message.Content = new ByteArrayContent(objByte);
                message.Content.Headers.ContentLength = objByte.Length;
                message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                message.Content.Headers.Add("Content-Disposition", "attachment; filename=" +obj.Name);
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

                string dirfmt = string.Format("{0}{1}/{2}", HttpContext.Current.Server.MapPath("~/Uploads/"), version.Release, obj.Name);
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


        #endregion

        #region post
        [Route("api/Cliente/{idClientes:int}/Version/{idVersion:int}/ReportarTodasTareas")]
        [HttpPost]
        public Object PostReportTodasTareas(int idClientes, int idVersion, [FromBody]List<ProcessMsg.Model.TareaBo> tareas)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            try
            {

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
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (version.Release == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (version.Fecha == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (version.Estado.ToString() == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var obj = ProcessMsg.Version.AddVersion(version);
                    if (obj == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
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
        [HttpPost]
        public Object PostComponentes(int idVersion, [FromBody]ProcessMsg.Model.AtributosArchivoBo archivo)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (archivo == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (archivo.Name == null || archivo.Modulo == null || archivo.LastWrite == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var obj = ProcessMsg.Componente.AddComponente(idVersion, archivo);
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

        [Route("api/Version/{idVersion:int}/LComponentes")]
        [HttpPost]
        public Object PostListaComponentes(int idVersion, [FromBody]IEnumerable<ProcessMsg.Model.AtributosArchivoBo> archivo)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                return Content(response.StatusCode, (ProcessMsg.Model.AtributosArchivoBo)null);
            }
            catch (Exception ex)
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
        [Route("api/ReportarTareaManual")]
        [HttpPut]
        public Object ReportarTareaManual([FromBody]ProcessMsg.Model.TareaBo tarea)
        {
            try
            {
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
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (version.Release == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (version.Fecha == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var obj = ProcessMsg.Version.UpdVersion(idVersion, version);
                    if (obj == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
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
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (ProcessMsg.Version.DelVersion(idVersion) <= 0)
                {
                    response.StatusCode = HttpStatusCode.Accepted;
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
                }

                return Content(response.StatusCode, (ProcessMsg.Model.AtributosArchivoBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        #endregion
    }
}
