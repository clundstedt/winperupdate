using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProcessMsg;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class SeguridadController : ApiController
    {
        #region get
        // GET: api/Seguridad
        [HttpGet]
        public Object Get(string mail, string password)
        {
            try
            {
                var obj = ProcessMsg.Seguridad.GetUsuario(mail);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.RegionBo)null);
                }

                return obj.Clave.Equals(ProcessMsg.Utils.Encriptar(password)) ? obj : null;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [HttpGet]
        [Route("api/Usuarios")]
        public Object Get()
        {
            try
            {
                var obj = ProcessMsg.Seguridad.GetUsuarios();
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.RegionBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [HttpGet]
        [Route("api/Usuarios/{idUsuario:int}")]
        public Object Get(int idUsuario)
        {
            try
            {
                var obj = ProcessMsg.Seguridad.GetUsuarios().SingleOrDefault(x => x.Id == idUsuario);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.RegionBo)null);
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
        [HttpPost]
        [Route("api/Usuarios")]
        public Object Post([FromBody]ProcessMsg.Model.UsuarioBo usuario)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (usuario.Persona.Nombres == null || usuario.Persona.Apellidos == null || usuario.Persona.Mail == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (usuario.CodPrf == 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    if (usuario.Clave == null)
                    {
                        usuario.Clave = ProcessMsg.Utils.Encriptar("12345678");
                    }
                    var persona = ProcessMsg.Seguridad.AddPersona(usuario.Persona);
                    if (persona == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        usuario.Persona.Id = persona.Id;
                        var obj = ProcessMsg.Seguridad.AddUsuario(usuario);
                        if (obj == null)
                        {
                            response.StatusCode = HttpStatusCode.Accepted;
                        }
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

        #endregion

        #region put
        [HttpPut]
        [Route("api/Usuarios/{id:int}")]
        public Object Put(int id, [FromBody]ProcessMsg.Model.UsuarioBo usuario)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (usuario.Persona.Nombres == null || usuario.Persona.Apellidos == null || usuario.Persona.Mail == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (usuario.CodPrf == 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var persona = ProcessMsg.Seguridad.UpdPersona(usuario.Persona);
                    if (persona == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        usuario.Persona.Id = persona.Id;
                        var obj = ProcessMsg.Seguridad.UpdUsuario(usuario);
                        if (obj == null)
                        {
                            response.StatusCode = HttpStatusCode.Accepted;
                        }
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
        #endregion
    }
}
