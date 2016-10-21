using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinPerUpdateAdmin.Controllers.Clientes
{
    public class AmbientesCltController : Controller
    {
        // GET: AmbientesClt
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

            ViewBag.Menu = "AmbientesClt";

            return View();
        }

        public PartialViewResult Ambientes()
        {
            return PartialView();
        }

        public PartialViewResult Crear()
        {
            return PartialView();
        }
    }
}