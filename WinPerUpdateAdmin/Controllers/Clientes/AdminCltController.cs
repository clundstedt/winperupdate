using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinPerUpdateAdmin.Controllers.Clientes
{
    public class AdminCltController : Controller
    {
        // GET: AdminClt
        public ActionResult Index()
        {
            if (Session["token"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }

            var usuario = ProcessMsg.Seguridad.GetUsuario(int.Parse(Session["token"].ToString()));
            if (usuario == null)
            {
                return RedirectToAction("Logout", "Home");
            }

            ViewBag.Menu = "AdminClt";

            return View();
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