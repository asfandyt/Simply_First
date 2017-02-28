using Simply_First.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Simply_First.Controllers
{   
    //[Authorize]
    public class UsersController : Controller
    {
        private SimplyFirstVMContext db = new SimplyFirstVMContext();
        // GET: Users
        public ActionResult Index()
        {
            return View();
            
        }

        // GET: Products
        public ActionResult BuyProducts()
        {
            return View(db.Products.ToList());
        }

        // GET: Products
        public ActionResult CustomerInvoice(string name)
        {

            return View(db.Products.ToList());

        }


        //GET: Users/Purchase
        public ActionResult Purchase(int? id)
        {
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        
        }

        // Secure Area
        public ActionResult SecureArea()
        {
            Page_Load();
            return View();
        }

        protected void Page_Load()
        {
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetNoStore();
        }
    }
}