using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class ClientesController : ApiController
    {
        #region get
        [Route("api/Clientes")]
        [HttpGet]
        public Object Get()
        {
            try
            {
                var obj = ProcessMsg.Cliente.GetClientes();
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.RegionBo)null);
                }

                return obj.OrderBy(x => x.Nombre);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Clientes/{idCliente:int}")]
        [HttpGet]
        public Object GetCliente(int idCliente)
        {
            try
            {
                var obj = ProcessMsg.Cliente.GetClientes().SingleOrDefault(x => x.Id == idCliente);
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

        [Route("api/Clientes/{idCliente:int}/Versiones")]
        [HttpGet]
        public Object GetVersionesCliente(int idCliente)
        {
            try
            {
                var obj = ProcessMsg.Cliente.GetVersiones(idCliente, null);
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

        [Route("api/Clientes/{idCliente:int}/Usuarios")]
        [HttpGet]
        public Object GetUsuariosCliente(int idCliente)
        {
            try
            {
                var obj = ProcessMsg.Cliente.GetUsuarios(idCliente);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.UsuarioBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Clientes/{idCliente:int}/Usuarios/{idUsuario:int}")]
        [HttpGet]
        public Object GetUsuariosCliente(int idCliente, int idUsuario)
        {
            try
            {
                var obj = ProcessMsg.Cliente.GetUsuarios(idCliente).SingleOrDefault(x => x.Id == idUsuario);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.UsuarioBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Region")]
        [HttpGet]
        public Object GetRegiones()
        {
            try
            {
                var obj = ProcessMsg.Cliente.GetRegiones();
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.RegionBo)null);
                }

                return obj.OrderBy(x => x.NomRgn);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Region/{idRgn:int}/Comunas")]
        [HttpGet]
        public Object GetComunas(int idRgn)
        {
            try
            {
                var obj = ProcessMsg.Cliente.GetComunas(idRgn);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.ComunaBo)null);
                }

                return obj.OrderBy(x => x.NomCmn);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Key/{Folio:int}/{EstMtc:int}/{MesIni}/{NroTrbc}/{NroTrbh}/{NroUsr}")]
        [HttpGet]
        public Object GetKeyCliente(int Folio, int EstMtc, string MesIni, string NroTrbc, string NroTrbh, string NroUsr)
        {
            try
            {
                var key = ProcessMsg.Utils.GenerarLicencia(Folio, EstMtc, MesIni, NroTrbc, NroTrbh, NroUsr);
                return Content(HttpStatusCode.OK, key); ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Clientes/Key")]
        [HttpGet]
        public Object GetFolio()
        {
            try
            {
                int folio = ProcessMsg.Cliente.GetFolioLicencia();
                return folio;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        #endregion



        #region post
        [Route("api/Clientes")]
        [HttpPost]
        public Object Post([FromBody]ProcessMsg.Model.ClienteBo cliente)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (cliente.Rut == 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Dv.ToString() == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Nombre.ToString() == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Comuna == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var obj = ProcessMsg.Cliente.Add(cliente);
                    if (obj == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, obj);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.VersionBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Clientes/{idCliente:int}/Usuarios")]
        [HttpPost]
        public Object PostUsuario(int idCliente, [FromBody]ProcessMsg.Model.UsuarioBo usuario)
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
                        usuario.Clave = ProcessMsg.Utils.Encriptar(ProcessMsg.Utils.RandomString(10));
                    }
                    var persona = ProcessMsg.Seguridad.AddPersona(usuario.Persona);
                    if (persona == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        usuario.Persona.Id = persona.Id;
                        var obj = ProcessMsg.Seguridad.AddUsuarioCliente(usuario);
                        if (obj == null)
                        {
                            response.StatusCode = HttpStatusCode.Accepted;
                        }

                        if (ProcessMsg.Cliente.AddUsuario(idCliente, obj.Id) == 0)
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
        [Route("api/Clientes/{id:int}")]
        [HttpPut]
        public Object Put(int id, [FromBody]ProcessMsg.Model.ClienteBo cliente)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (id <= 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Rut == 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Dv.ToString() == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Nombre.ToString() == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Comuna == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var obj = ProcessMsg.Cliente.Update(id, cliente);
                    if (obj == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, obj);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.VersionBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [HttpPut]
        [Route("api/Clientes/{id:int}/Usuarios/{idUsuario:int}")]
        public Object Put(int id, int idUsuario, [FromBody]ProcessMsg.Model.UsuarioBo usuario)
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
                        usuario.Id = idUsuario;
                        ProcessMsg.Seguridad.UpdUsuario(usuario);

                        var obj = ProcessMsg.Cliente.GetUsuarios(id).SingleOrDefault(x => x.Id == idUsuario);
                        if (obj == null)
                        {
                            response.StatusCode = HttpStatusCode.Accepted;
                        }
                        return Content(response.StatusCode, obj);
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

        #region delete
        [Route("api/Clientes/{id:int}")]
        [HttpDelete]
        public Object Delete(int id)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (id <= 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    if (ProcessMsg.Cliente.Delete(id) <= 0)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }

                    return Content(response.StatusCode, (ProcessMsg.Model.ClienteBo)null);
                }

                return Content(response.StatusCode, (ProcessMsg.Model.ClienteBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        #endregion
    }
}
