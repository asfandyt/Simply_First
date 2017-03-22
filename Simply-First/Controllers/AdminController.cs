using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Simply_First.Models;
using Newtonsoft.Json.Linq;

namespace Simply_First.Controllers
{
    [Authorize(Roles = "20d75ec5-13d8-451e-aaca-154a64111711")]
    public class AdminController : Controller
    {
        private SimplyFirstVMContext db = new SimplyFirstVMContext();

        [Authorize(Roles = "20d75ec5-13d8-451e-aaca-154a64111711")]
        public ActionResult Index()
        {
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new SimplyFirstVMContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SimplyFirstVMContext()));

            var user = userManager.Users.ToList();
            var role = roleManager.Roles.ToList();
            
            // This code could go in a repo.
            List<SiteUserVM> siteUsers = new List<SiteUserVM>();

            foreach (var users in user)
            {
                SiteUserVM siteUser = new SiteUserVM();
                siteUser.UserId = users.Id;
                siteUser.Email = users.Email;
                siteUser.UserName = users.UserName;
                siteUser.EmailConfirmed = users.EmailConfirmed;
                siteUser.TwoFactorEnabled = users.TwoFactorEnabled;
                siteUser.PhoneNumber = users.PhoneNumber;

                foreach (var roles in role)
                {
                    siteUser.RoleName = roles.Name;
                }

                siteUsers.Add(siteUser);
            }
            
            return View(siteUsers);
        }

        [Authorize(Roles = "20d75ec5-13d8-451e-aaca-154a64111711")]
        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }

        [Authorize(Roles = "20d75ec5-13d8-451e-aaca-154a64111711")]
        [HttpPost]
        public ActionResult AddRole(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                // *** New: Connect to AspNetRole using code first.
                using (var db = new SimplyFirstVMContext())
                {
                    var role = new IdentityRole
                    {
                        Id = roleVM.RoleName,
                        Name = roleVM.RoleName
                    };

                    db.Roles.Add(role);
                    db.SaveChanges();

                    TempData["AddRoleSuccess"] = "Added '" + role.Id + "' to the Simply First Role!";
                    return RedirectToAction("AddRole");
                }
            }

            return View(roleVM);
        }

        [Authorize(Roles = "20d75ec5-13d8-451e-aaca-154a64111711")]
        [HttpGet]
        public ActionResult AddUserToRole()
        {
            return View();
        }

        [Authorize(Roles = "20d75ec5-13d8-451e-aaca-154a64111711")]
        [HttpPost]
        public ActionResult AddUserToRole(UserRoleVM userRoleVM)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SimplyFirstVMContext())
                {
                    var user = db.Users.Where(e => e.Email == userRoleVM.Email).FirstOrDefault();
                    //var role = db.Roles.Where(r => r.Name == userRoleVM.RoleName).FirstOrDefault();

                    //var userRole = new IdentityRole();
                    //userRole.Id = role.Id;
                    //userRole.Name = user.Id;
                    //db.Roles.Add(userRole);
                    //db.SaveChanges();

                    var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new SimplyFirstVMContext()));

                    if (user == null) return RedirectToAction("AddUserToRole");

                    userManager.AddToRoles(user.Id, new string[] { userRoleVM.RoleName });

                    TempData["AddUserToRoleSuccess"] = "Added '" + user.Email + "' to the " + userRoleVM.RoleName + " Simply First Role!";

                    return RedirectToAction("AddUserToRole");
                }
            }

            return View(userRoleVM);
        }
    }
}