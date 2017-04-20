using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Simply_First.ViewModels;

namespace Simply_First.Controllers
{
    public class UsersController : Controller
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
        [HttpGet]
        public ActionResult Create()
        {
            string userId = FindUserId();

            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserInformation userInformation = db.UserInformation.Where(u => u.UserId == userId).FirstOrDefault();

            if (ModelState.IsValid && userInformation == null)
            {
                
                // The form should display anyway
                userInformation = new UserInformation
                {
                    UserId = userId,
                    FirstName = " ",
                    LastName = " ",
                    PhoneNumber = " ", 
                    StreetAddress = " ",
                    City = " ",
                    PostalCode = " ",
                    Province = " ",
                    Country = " ",
                    JoinDate = DateTime.Now
                };
                
                db.UserInformation.Add(userInformation);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                        }
                    }
                }

                return View(userInformation);
            }

            return View(userInformation);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserInformation model)
        {
            string userId = FindUserId();

            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserInformation user = db.UserInformation.Where(u => u.UserId == userId).FirstOrDefault();
            
            
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    UserInformation newUser = new UserInformation
                    {
                        UserId = userId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        StreetAddress = model.StreetAddress,
                        City = model.City,
                        PostalCode = model.PostalCode,
                        Province = model.Province,
                        Country = model.Country,
                        JoinDate = model.JoinDate
                    };
                    db.UserInformation.Add(newUser);
                }
                else
                {
                    try
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.PhoneNumber = model.PhoneNumber;
                        user.StreetAddress = model.StreetAddress;
                        user.City = model.City;
                        user.PostalCode = model.PostalCode;
                        user.Province = model.Province;
                        user.Country = model.Country;
                        user.JoinDate = model.JoinDate;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit()
        {
            string id = FindUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserInformation userInformation = db.UserInformation.Where(u => u.UserId == id).FirstOrDefault();

            if (userInformation == null)
            {
                return HttpNotFound();
            }

            return View(userInformation);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserInformation model)
        {
            string userId = FindUserId();

            if (ModelState.IsValid)
            {
                UserInformation info = new UserInformation
                {
                    UserId = userId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    StreetAddress = model.StreetAddress,
                    City = model.City,
                    PostalCode = model.PostalCode,
                    Province = model.Province,
                    Country = model.Country,
                    JoinDate = DateTime.Now
                };

                db.UserInformation.Add(info);
                db.SaveChanges();
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        public ActionResult Search(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string userName = User.Identity.Name;
                IdentityUser user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                string userId = user.Id;
                IEnumerable<PayPal> paypal = db.PayPal.Where(s => s.custom == userId && s.amount.ToString().Contains(name) || s.custom == userId && s.txtTime.ToString().Contains(name));
                return View(paypal.ToList());
            }

            return View();
        }
    }
}
