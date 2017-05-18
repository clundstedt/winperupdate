using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProcessMsg;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class SeguridadController : ApiController
    {
        #region Classes
        public class WebConfig
        {
            public string userbd { get; set; }
            public string passbd { get; set; }
            public string svbd { get; set; }
            public string nombrebd { get; set; }
            //---
            public ProcessMsg.Model.UsuarioBo Usuario { get; set; }
            //--
            public string pathGenSetup  { get; set; }
            public string hostMail { get; set; }
            public string userMail { get; set; }
            public string pwdMail { get; set; }
            public string fromMail { get; set; }
            public string correoSoporte { get; set; }
            public string upload { get; set; }
            public string voficial { get; set; }
            public string fuentes { get; set; }
        }

        #endregion

        #region get
        [Route("api/Config")]
        [HttpGet]
        public Object GetConfig()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return new WebConfig
                {
                    pathGenSetup = ConfigurationManager.AppSettings["pathGenSetup"],
                    hostMail = ConfigurationManager.AppSettings["hostMail"],
                    userMail = ConfigurationManager.AppSettings["userMail"],
                    pwdMail = ConfigurationManager.AppSettings["pwdMail"],
                    fromMail = ConfigurationManager.AppSettings["fromMail"],
                    correoSoporte = ConfigurationManager.AppSettings["correoSoporte"],
                    upload = ConfigurationManager.AppSettings["upload"],
                    voficial = ConfigurationManager.AppSettings["voficial"],
                    fuentes = ConfigurationManager.AppSettings["fuentes"]
                };
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

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
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
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
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Seguridad.GetUsuario(idUsuario);
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

        [HttpGet]
        [Route("api/Usuarios/{idUsuario:int}/Cliente")]
        public Object GetCliente(int idUsuario)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Cliente.GetClienteUsuario(idUsuario);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.ClienteBo)null);
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
        
        [Route("api/CrearBD")]
        [HttpPost]
        public Object CrearBD([FromBody] WebConfig webConfig)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                webConfig.Usuario.Clave = Utils.Encriptar(webConfig.Usuario.Clave);

                var strConn = string.Format(@"Database={0};Server={1};User={2};Password={3};Connect Timeout=200;Integrated Security=;", webConfig.nombrebd, webConfig.svbd, webConfig.userbd, webConfig.passbd);
                SqlConnection conn = new SqlConnection();

                var sr = File.OpenText(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Fuentes/")) + "/Install/" + "bd_wpu.sql");
                string query = sr.ReadToEnd();
                sr.Close();

                // split script on GO command
                IEnumerable<string> commandStrings = Regex.Split(query, @"^\s*GO\s*$",
                                         RegexOptions.Multiline | RegexOptions.IgnoreCase);

                conn = new SqlConnection(strConn);
                conn.Open();

                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        using (var command = new SqlCommand(commandString, conn))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }

                SqlCommand cmd = new SqlCommand("insert into Personas (Nombres, Apellidos, Mail) output INSERTED.idPersonas values(@nombre, @apellido, @mail)", conn);
                cmd.Parameters.AddWithValue("@nombre",webConfig.Usuario.Persona.Nombres);
                cmd.Parameters.AddWithValue("@apellido", webConfig.Usuario.Persona.Apellidos);
                cmd.Parameters.AddWithValue("@mail", webConfig.Usuario.Persona.Mail);
                webConfig.Usuario.Persona.Id = (int)cmd.ExecuteScalar();
                cmd = new SqlCommand("insert into Usuarios (idPersonas, CodPrf, Clave, EstUsr) values(@idPersona, @codPrf, @clave, @estado)", conn);
                cmd.Parameters.AddWithValue("@idPersona", webConfig.Usuario.Persona.Id);
                cmd.Parameters.AddWithValue("@codPrf", webConfig.Usuario.CodPrf);
                cmd.Parameters.AddWithValue("@clave",webConfig.Usuario.Clave);
                cmd.Parameters.AddWithValue("@estado",webConfig.Usuario.EstUsr);
                cmd.ExecuteNonQuery();

                conn.Close();
                Configuration cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                cfg.ConnectionStrings.ConnectionStrings["ApplicationServices"].ConnectionString = Utils.Encriptar(strConn);
                cfg.Save();

                
                return Content(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [HttpPost]
        [Route("api/Usuarios")]
        public Object Post([FromBody]ProcessMsg.Model.UsuarioBo usuario)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (usuario.Persona.Nombres == null || usuario.Persona.Apellidos == null || usuario.Persona.Mail == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (usuario.CodPrf == 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    if (usuario.Clave == null)
                    {
                        usuario.Clave = ProcessMsg.Utils.Encriptar(Utils.RandomString(8));
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
        [Route("api/GuardarConfig")]
        [HttpPut]
        public Object PutConfig([FromBody] WebConfig webConfig)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                Configuration cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

                cfg.AppSettings.Settings["pathGenSetup"].Value = webConfig.pathGenSetup;
                cfg.AppSettings.Settings["hostMail"].Value = webConfig.hostMail;
                cfg.AppSettings.Settings["userMail"].Value = webConfig.userMail;
                cfg.AppSettings.Settings["pwdMail"].Value = webConfig.pwdMail;
                cfg.AppSettings.Settings["fromMail"].Value = webConfig.fromMail;
                cfg.AppSettings.Settings["correoSoporte"].Value = webConfig.correoSoporte;
                cfg.AppSettings.Settings["upload"].Value = webConfig.upload;
                cfg.AppSettings.Settings["voficial"].Value = webConfig.voficial;
                cfg.AppSettings.Settings["fuentes"].Value = webConfig.fuentes;
                cfg.Save();
                return Content(HttpStatusCode.OK, true);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        // PUT: api/Seguridad
        [HttpPut]
        public Object Put(string mail, string password, string nPassword)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Seguridad.GetUsuario(mail);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.RegionBo)null);
                }
                string claveEnc = ProcessMsg.Utils.Encriptar(password);
                if (obj.Clave.Equals(claveEnc))
                {
                    var objUpd = ProcessMsg.Seguridad.UpdUsuario(obj.Id, Utils.Encriptar(nPassword));
                    if (objUpd == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    return Content(response.StatusCode, objUpd);
                }
                return Content(response.StatusCode, (ProcessMsg.Model.UsuarioBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        [HttpPut]
        [Route("api/Usuarios/{id:int}")]
        public Object Put(int id, [FromBody]ProcessMsg.Model.UsuarioBo usuario)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
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
                        usuario.Id = id;
                        var obj = ProcessMsg.Seguridad.UpdUsuario(usuario);
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
    }
}
