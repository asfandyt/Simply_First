using Microsoft.AspNet.Identity.EntityFramework;
using Simply_First.Services;
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

        public string FindUserId()
        {
            string name = User.Identity.Name;

            IdentityUser user = db.Users.Where(u => u.UserName == name).FirstOrDefault();
            string userId = user.Id;

            return userId;
        }

        [Authorize]
        // GET: PayPal
        public ActionResult InvoiceHistory()
        {
            string userId = FindUserId();

            IEnumerable<PayPal> user = db.PayPal.Where(u => u.custom == userId).ToList();

            return View(user.OrderByDescending(t => t.txtTime));
        }

        public ActionResult Purchase()
        {
            PayPalService paypalService = new PayPalService("test");

            if (paypalService.TXN_ID != null)
            {
                PayPal paypal = new PayPal();
                paypal.transactionID = paypalService.TXN_ID;
                decimal amount = Convert.ToDecimal(paypalService.PaymentGross);
                paypal.amount = amount;
                paypal.buyerEmail = paypalService.PayerEmail;
                paypal.txtTime = DateTimeOffset.Now.LocalDateTime;
                paypal.quantity = paypalService.Quantity;
                paypal.firstName = paypalService.PayerFirstName;
                paypal.lastName = paypalService.PayerLastName;
                paypal.custom = paypalService.Custom;
                ShoppingCart.Instance.ClearCart();
                db.PayPal.Add(paypal);
                db.SaveChanges();

            }

            return View("Success", "Home");
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }



    }
}