using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinPerUpdateAdmin.Controllers.Clientes
{
    public class AdminCltController : Controller
    {
        private char TipoPerfil = 'E';
        // GET: AdminClt
        public ActionResult Index()
        {
            try
            {
                ViewBag.Menu = "AdminClt";
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

        public PartialViewResult Versiones()
        {
            return PartialView();
        }

        public PartialViewResult Componentes()
        {
            return PartialView();
        }
    }
}