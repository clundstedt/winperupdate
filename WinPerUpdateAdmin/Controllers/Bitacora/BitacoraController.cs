using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinPerUpdateAdmin.Controllers.Bitacora
{
    public class BitacoraController : Controller
    {
        public char TipoPerfil = 'I';//Interno
        // GET: Bitacora
        public ActionResult Index()
        {
            try
            {
                ViewBag.Menu = "Bitacora";
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

        public PartialViewResult Clientes()
        {
            return PartialView();
        }

        public PartialViewResult Usuario()
        {
            return PartialView();
        }

        public PartialViewResult Modulo()
        {
            return PartialView();
        }

        public PartialViewResult Version()
        {
            return PartialView();
        }
    }
}