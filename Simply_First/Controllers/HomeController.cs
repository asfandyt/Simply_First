using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Simply_First.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Register() {
            return View();
        }

        public ActionResult Admin() {
            return View();
        }

        public ActionResult Employee()
        {
            return View();
        }

        public ActionResult Customer()
        {
            return View();
        }

        public ActionResult Product() {
            return View();
        }
    }
}