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
            Session["token"] = "1";

            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();

            return RedirectToAction("Index", "Home");
        }
    }
}