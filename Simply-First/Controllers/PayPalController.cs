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
            return View(db.PayPal.OrderByDescending(t => t.TransactionTime));
        }

        [Authorize]
        public ActionResult Purchase()
        {
            PayPalService paypalService = new PayPalService("test");

            if (paypalService.TXN_ID != null)
            {
                PayPal paypal = new PayPal();
                paypal.TransactionId = paypalService.TXN_ID;
                decimal amount = Convert.ToDecimal(paypalService.PaymentGross);
                paypal.Amount = amount;
                paypal.BuyerEmail = paypalService.PayerEmail;
                paypal.TransactionTime = DateTimeOffset.Now.LocalDateTime;
                paypal.Quantity = paypalService.Quantity;
                paypal.FirstName = paypalService.PayerFirstName;
                paypal.LastName = paypalService.PayerLastName;
                paypal.Custom = paypalService.Custom;

                db.PayPal.Add(paypal);
                db.SaveChanges();
            }

            return View();
        }
    }
}