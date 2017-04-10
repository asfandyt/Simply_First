using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserInformation model)
        {
            string userId = FindUserId();

            if (ModelState.IsValid)
            {
                UserInformation info = new UserInformation
                {
                    Id = model.Id,
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
