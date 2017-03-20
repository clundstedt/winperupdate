using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinPerUpdateAdmin.Controllers.Seguridad
{
    public class SeguridadController : Controller
    {
        private char TipoPerfil = 'I';
        // GET: Seguridad
        public ActionResult Index()
        {
            try
            {
                ViewBag.Menu = "Seguridad";
                if (Session["token"] == null)
                {
                    return RedirectToAction("Logout", "Home");
                }

                var usuario = ProcessMsg.Seguridad.GetUsuario(int.Parse(Session["token"].ToString()));
                if (usuario == null)
                {
                    return RedirectToAction("Logout", "Home");
                }
                var menus = ProcessMsg.Perfiles.GetMenus(usuario.Id);
                if (menus.Exists(x => x.Link.Contains(ViewBag.Menu)))
                {
                    var perfil = ProcessMsg.Perfiles.GetPerfil(usuario.CodPrf);
                    if (TipoPerfil == perfil.Tipo)
                    {
                        return View();
                    }
                }
                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public PartialViewResult Inicio()
        {
            return PartialView();
        }

        public PartialViewResult CrearUsuario()
        {
            return PartialView();
        }

        public PartialViewResult Configuracion()
        {
            return PartialView();
        }
    }
}