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

        public ActionResult Profile()
        {
            return View();
        }
        public ActionResult AddEmployee()
        {
            return View();
        }
        public ActionResult EditEmployee()
        {
            return View();
        }
        public ActionResult RemoveEmployee()
        {
            return View();
        }
        public ActionResult UpdateInventory()
        {
            return View();
        }
        public ActionResult CheckInventory()
        {
            return View();
        }
        public ActionResult CreateOrder()
        {
            return View();
        }
        public ActionResult AllCustomers()
        {
            return View();
        }
        public ActionResult CreateCustomer()
        {
            return View();

        }

        public ActionResult EditCustomer()
        {
            return View();

        }
        public ActionResult DeleteCustomer()
        {
            return View();

        }

        public ActionResult PurchaseProduct() {
            return View();
        }
    }
}