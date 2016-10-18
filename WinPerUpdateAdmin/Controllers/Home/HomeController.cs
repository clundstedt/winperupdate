using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinPerUpdateAdmin.Controllers.Home
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Login = "Login";
            return View();
        }

        public ActionResult AutorizarIngreso(string idUser)
        {
            var usuario = ProcessMsg.Seguridad.GetUsuario(idUser);

            if (usuario != null)
            {
                Session["token"] = usuario.Id.ToString();
                ViewBag.NomUser = usuario.Persona.NomFmt;
                if (usuario.CodPrf == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (usuario.CodPrf == 11)
                {
                    return RedirectToAction("Index", "AdminClt");
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SendMail(string mail)
        {
            var respuesta = new
            {
                CodErr = 0,
                MsgErr = "mail enviado exitosamente"
            };

            try
            {
                var usuario = ProcessMsg.Seguridad.GetUsuario(mail);

                if (usuario != null)
                {
                    string mensaje = ProcessMsg.Utils.ReadPlantilla(Server.MapPath("~/Content/html/recuperar.htm"));
                    mensaje = mensaje.Replace("@clave@", ProcessMsg.Utils.DesEncriptar(usuario.Clave));

                    if (!ProcessMsg.Utils.SendMail(mensaje, "Activación cuenta Winper Update", mail))
                    {
                        respuesta = new
                        {
                            CodErr = 2,
                            MsgErr = "No se ha podido enviar la clave al correo indicado. Intente más tarde ..."
                        };
                    }
                }

                return Json(respuesta);
            }
            catch (Exception ex)
            {
                return Json(new { CodErr = 1, MsgErr = ex.Message });
            }
        }

    }
}