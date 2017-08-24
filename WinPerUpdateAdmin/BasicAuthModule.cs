using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

namespace WinPerUpdateAdmin
{
    public class BasicAuthModule : IHttpModule
    {
        #region Antiguo
        /*
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnAuthenticateRequest;
        }

        private static bool CheckPassword(string username, string password)
        {
            return username == "user" && password == "password";
        }

        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                var application = (HttpApplication)sender;
                var request = new HttpRequestWrapper(application.Request);

                var headers = request.Headers;
                var authData = headers["Authorization"];
                if (!string.IsNullOrEmpty(authData))
                {
                    var scheme = authData.Substring(0, authData.IndexOf(' '));
                    var parameter = authData.Substring(scheme.Length).Trim();
                    var userPwd = Encoding.UTF8.GetString(Convert.FromBase64String(parameter));
                    var user = userPwd.Substring(0, userPwd.IndexOf(':'));
                    var password = userPwd.Substring(userPwd.IndexOf(':') + 1);
                    // Validamos user y password (aquí asumimos que siempre son ok)

                    if (CheckPassword(user, password))
                    {
                        var principal = new GenericPrincipal(new GenericIdentity(user), null);
                        PutPrincipal(principal);
                    }
                    else
                    {
                        // Invalid username or password.
                        HttpContext.Current.Response.StatusCode = 401;
                    }
                }
            }
            catch(Exception)
            {
                HttpContext.Current.Response.StatusCode = 401;
            }
        }

        public void Dispose()
        {

        }

        private void PutPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
        */
        #endregion

        private const string Realm = "My Realm";

        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        // TODO: Here is where you would validate the username and password.
        private static bool CheckPassword(int idPersona, string mail, string pass, DateTime fecha, ProcessMsg.Model.UsuarioBo user)
        {
            var usr = ProcessMsg.Seguridad.GetUsuario(mail);

            return (user.Persona.Id == idPersona
                && user.Persona.Mail.Equals(mail)
                && pass.Equals(usr.Clave)
                && fecha.Day == DateTime.Now.Day
                && fecha.Month == DateTime.Now.Month
                && fecha.Year == DateTime.Now.Year);
        }

        private static void AuthenticateUser(string credentials)
        {
            try
            {
                var encoding = Encoding.GetEncoding("iso-8859-1");
                credentials = encoding.GetString(Convert.FromBase64String(credentials));
                var s = ProcessMsg.Utils.G_Desencripta(credentials).Split(':');
                var idUser = int.Parse(s[0]);
                var idPersona = int.Parse(s[1]);
                var mail = s[2];
                var pass = s[3];
                var fecha = Convert.ToDateTime(s[4]);
                var user = ProcessMsg.Seguridad.GetUsuario(idUser);

                if (CheckPassword(idPersona, mail, pass, fecha, user))
                {
                    var identity = new GenericIdentity(user.Persona.Nombres);
                    SetPrincipal(new GenericPrincipal(identity, null));
                    HttpContext.Current.Response.StatusCode = 200;
                }
                else
                {
                    // Invalid username or password.
                    HttpContext.Current.Response.StatusCode = 401;
                }
            }
            catch (FormatException)
            {
                // Credentials were not formatted correctly.
                HttpContext.Current.Response.StatusCode = 401;
            }
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal.Scheme.Equals("basic",
                        StringComparison.OrdinalIgnoreCase) &&
                    authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }

        // If the request was unauthorized, add the WWW-Authenticate header 
        // to the response.
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate",
                    string.Format("Basic realm=\"{0}\"", Realm));
            }
            
        }

        public void Dispose()
        {
        }
    }
}