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

        [Authorize]
        // GET: PayPal
        public ActionResult InvoiceHistory()
        {
            return View(db.PayPal.OrderByDescending(t => t.txtTime));
        }

        [Authorize]
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

                db.PayPal.Add(paypal);
                db.SaveChanges();
            }

            return View();
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