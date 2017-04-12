﻿using Microsoft.AspNet.Identity;
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

        //public void AddToCart(object sender, EventArgs e)
        //{
        //    // Add product 1 to the shopping cart
        //    ShoppingCart.Instance.AddItem(1);

        //    // Redirect the user to view their shopping cart
        //   // Response.Redirect("ViewCart.aspx");
        //}

        public ActionResult AddtoCart(int id)
        {
            ShoppingCart.Instance.AddItem(id);
            //Console.WriteLine("SOMETHING HAPPENED");
            return RedirectToAction("Purchase", "Home");
        }

        public ActionResult ViewCart()
        {
            // Build an example model

            // var model = ShoppingCart.Instance;
            // var model = new ShoppingVM();

            ViewBag.Shopping = ShoppingCart.Instance.Items.ToList();


            ///model.Property = Session["ASPNETShoppingCart"];

            // List<Array> shitList = new List<Array> {}
            // Pass the model to the View
            return View();
        }
        public ActionResult ApiDocumentation()
        {
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

        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {

            ShoppingCart.Instance.SetItemQuantity(id, quantity);
            return RedirectToAction("ViewCart", "Home");
        }
    }
}