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
