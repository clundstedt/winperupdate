using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class ModuloController : ApiController
    {
        #region get
        // GET: api/Modulos
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

        #endregion

    }
}
