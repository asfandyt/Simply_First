using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Simply_First.Models;

namespace Simply_First.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddRole(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                // *** New: Connect to AspNetRole using code first.
                using (var db = new SimplyFirstVMContext())
                {
                //    AspNetRoles role = new AspNetRoles();
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

        [Authorize]
        [HttpGet]
        public ActionResult AddUserToRole()
        {
            return View();
        }

        [Authorize]
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