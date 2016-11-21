using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinPerUpdateAdmin.Controllers.Seguridad
{
    public class SeguridadController : Controller
    {
        // GET: Seguridad
        public ActionResult Index()
        {
            if (Session["token"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            ViewBag.Menu = "Seguridad";
            return View();
        }

        public PartialViewResult Inicio()
        {
            return PartialView();
        }

        public PartialViewResult CrearUsuario()
        {
            return PartialView();
        }
    }
}