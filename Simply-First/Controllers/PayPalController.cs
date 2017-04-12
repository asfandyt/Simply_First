using Simply_First.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Simply_First.Controllers
{
    public class PayPalController : Controller
    {
        private SimplyFirstVMContext db = new SimplyFirstVMContext();

        // GET: PayPal
        public ActionResult Index()
        {
            return View(db.PayPal.OrderByDescending(t => t.TransactionTime));
        }
    }
}