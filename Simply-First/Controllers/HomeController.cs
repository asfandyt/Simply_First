using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Simply_First.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Simply_First.Services;
using Simply_First.Repositories;
using System.Collections.Generic;

namespace Simply_First.Controllers
{
    public class HomeController : Controller
    {
        private SimplyFirstVMContext db = new SimplyFirstVMContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IEnumerable<Products> product = db.Products.Where(s => s.ProductName.Contains(name) || s.Manufacturer.Contains(name));
                return View(product.ToList());
            }
            return View(db.Products.ToList());
        }

        public ActionResult Product()
        {
            return View();
        }

        public ActionResult Purchase()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Document()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Contact(Contact contactEmail)
        {

            String Email = contactEmail.Email;
            String Name = contactEmail.Name;
            String Message = contactEmail.Message;

            if (ModelState.IsValid)
            {

                EmailRepo emailRepo = new EmailRepo();

                //prepare email
                var fromAddress = Email.ToString();

                //start email Thread
                // var tEmail = new Thread(() =>
                emailRepo.SendEmail(fromAddress, Message, Name);
                // tEmail.Start();

                TempData["ContactSuccess"] = "Form successfully sent!";

                return RedirectToAction("Contact");
            }

            TempData["ContactError"] = "Please provide proper details!";

            return View();
        }

        public ActionResult API()
        {
            return View();
        }

        public ActionResult AddtoCart(int id)
        {
            ShoppingCart.Instance.AddItem(id);

            return RedirectToAction("Purchase", "Home");
        }

        public ActionResult ViewCart()
        {
            string name = User.Identity.Name;

            IdentityUser user = db.Users.Where(u => u.UserName == name).FirstOrDefault();

            ViewBag.Shopping = ShoppingCart.Instance.Items.ToList();

            if(Request.IsAuthenticated)
            {
                Session["session_tx"] = "Unique Session Id";
                ViewBag.UserId = user.Id;
            }

            return View();
        }


        public ActionResult RemoveItem(int id)
        {
            ShoppingCart.Instance.RemoveItem(id);
            return RedirectToAction("ViewCart", "Home");
        }

        public ActionResult AddItem(int id)
        {
            ShoppingCart.Instance.AddItem(id);
            return RedirectToAction("ViewCart", "Home");
        }

        public ActionResult SubtractItem(int id)
        {
            ShoppingCart.Instance.SubtractItem(id);
            return RedirectToAction("ViewCart", "Home");
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            ShoppingCart.Instance.SetItemQuantity(id, quantity);
            return RedirectToAction("ViewCart", "Home");
        }
    }
}
