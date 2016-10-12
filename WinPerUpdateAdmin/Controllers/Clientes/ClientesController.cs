using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinPerUpdateAdmin.Controllers.Clientes
{
    public class ClientesController : Controller
    {
        // GET: Clientes
        public ActionResult Index()
        {
            if (Session["token"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            ViewBag.Menu = "Clientes";
            return View();
        }

        public PartialViewResult Inicio()
        {
            return PartialView();
        }

        public PartialViewResult Crear()
        {
            return PartialView();
        }
    }
}