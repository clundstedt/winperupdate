using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class SuitesController : ApiController
    {
        #region Gets
        [Route("api/Suites")]
        [HttpGet]
        public Object GetSuites()
        {
            try
            {
                return ProcessMsg.Suites.GetSuites();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        #endregion
    }
}
