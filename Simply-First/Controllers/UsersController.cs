using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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

            if (userInformation == null)
            {
                
                // The form should display anyway
                userInformation = new UserInformation
                {
                    UserId = userId,
                    FirstName = "a",
                    LastName = "a",
                    PhoneNumber = 111111111,
                    StreetAddress = "asd",
                    City = "asda",
                    PostalCode = "asd",
                    Province = "asd",
                    Country = "asd",
                    JoinDate = DateTime.Now
                };
                
                db.UserInformation.Add(userInformation);
                db.SaveChanges();

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

            UserInformation user = db.UserInformation.Where(u => u.UserId == userId).FirstOrDefault();
            
            
            if (ModelState.IsValid)
            {
                user.UserId = userId;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.StreetAddress = model.StreetAddress;
                user.City = model.City;
                user.PostalCode = model.PostalCode;
                user.Province = model.Province;
                user.Country = model.Country;

                //db.UserInformation.Add(user);
                db.SaveChanges();
                //db.Entry(info);
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
    }
}
