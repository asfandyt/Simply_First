using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Simply_First.ViewModels;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net;
using Simply_First.Repositories;

namespace Simply_First.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private SimplyFirstVMContext db = new SimplyFirstVMContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new SimplyFirstVMContext()));

            var users = userManager.Users.ToList();
            
            // This code could go in a repo.
            List<SiteUserVM> siteUsers = new List<SiteUserVM>();

            foreach (var user in users)
            {
                SiteUserVM siteUser = new SiteUserVM();
                UserInformation userInformation = db.UserInformation.Where(u => u.UserId == user.Id).FirstOrDefault();
                if (userInformation == null)
                {
                    userInformation = new UserInformation
                    {
                        UserId = user.Id,
                        FirstName = "",
                        LastName = "",
                        PhoneNumber = "",
                        StreetAddress = "",
                        City = "",
                        PostalCode = "",
                        Province = "",
                        Country = "",
                        JoinDate = DateTime.Now
                    };
                    //return HttpNotFound("User Information is null");
                }
                siteUser.Id = user.Id;
                siteUser.Email = user.Email;
                siteUser.UserName = user.UserName;
                siteUser.EmailConfirmed = user.EmailConfirmed;
                if (user.PhoneNumber != null)
                {
                    siteUser.PhoneNumber = user.PhoneNumber;
                }
                if (userInformation.FirstName != null)
                {
                    siteUser.FirstName = userInformation.FirstName;
                }
                siteUser.LastName = userInformation.LastName;
                siteUser.StreetAddress = userInformation.StreetAddress;
                siteUser.City = userInformation.City;
                siteUser.PostalCode = userInformation.PostalCode;
                siteUser.Province = userInformation.Province;
                siteUser.Country = userInformation.Country;
                siteUsers.Add(siteUser);
            }
            
            return View(siteUsers);
        }


        [Authorize(Roles = "Admin")]
        // GET: PayPal
        public ActionResult InvoiceHistory()
        {
            return View(db.PayPal.OrderByDescending(t => t.txtTime));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Search(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IEnumerable<PayPal> paypal = db.PayPal.Where(s => s.amount.ToString().Contains(name) || s.transactionID.Contains(name) || s.lastName.Contains(name) || s.firstName.Contains(name) || s.buyerEmail.Contains(name));
                return View(paypal.ToList());
            }

            return View(db.PayPal.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UserRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SimplyFirstVMContext()));

            var role = roleManager.Roles.ToList();

            List<SiteUsersRoleVM> siteUsersRoles = new List<SiteUsersRoleVM>();

            foreach (var roles in role)
            {
                SiteUsersRoleVM userRoles = new SiteUsersRoleVM();

                userRoles.UserId = roles.Id;
                userRoles.RoleId = roles.Name;
                userRoles.Users = roles.Users.ToList();
                siteUsersRoles.Add(userRoles);
            }

            return View(siteUsersRoles);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddRole(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                // *** New: Connect to AspNetRole using code first.
                using (var db = new SimplyFirstVMContext())
                {
                    IdentityRole role = new IdentityRole();

                    Guid g = Guid.NewGuid();

                    // See if role exists.
                    var existingRoles = db.Roles.Where(r => r.Id == g.ToString() || r.Name == roleVM.RoleName);

                    if (existingRoles.Count() > 0)
                    {
                        return View();
                    }
                    role.Id = g.ToString();
                    role.Name = roleVM.RoleName;
                    db.Roles.Add(role);
                    db.SaveChanges();

                    TempData["AddRoleSuccess"] = "Added '" + role.Id + "' to the Simply First Role!";
                    return RedirectToAction("AddRole");
                }
            }

            return View(roleVM);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AddUserToRole()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddUserToRole(UserRoleVM userRoleVM)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SimplyFirstVMContext())
                {
                    var user = db.Users.Where(e => e.Email == userRoleVM.Email).FirstOrDefault();

                    var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new SimplyFirstVMContext()));

                    if (user == null) return RedirectToAction("AddUserToRole");

                    userManager.AddToRoles(user.Id, new string[] { userRoleVM.RoleName });

                    TempData["AddUserToRoleSuccess"] = "Added '" + user.Email + "' to the " + userRoleVM.RoleName + " Simply First Role!";

                    return RedirectToAction("AddUserToRole");
                }
            }

            return View(userRoleVM);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditUserDetails(string id)
        {
            string userId = id;
            SiteUserVM siteUser = new SiteUserVM();
            var userManagerMain = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new SimplyFirstVMContext()));
            using (var db = new SimplyFirstVMContext())
            {
                var user = db.Users.Where(e => e.Id == id).FirstOrDefault();
                //UserInformation userInformation = db.UserInformation.Where(u => u.UserId == user.Id).FirstOrDefault();
                UserInformation userInfo = db.UserInformation.Where(u => u.UserId == userId).FirstOrDefault();
                
                if (user != null)
                {
                    if (userInfo != null)
                    {
                        siteUser = new SiteUserVM
                        {
                            Id = userId,
                            UserName = user.UserName,
                            Email = user.Email,
                            EmailConfirmed = user.EmailConfirmed,
                            FirstName = userInfo.FirstName,
                            LastName = userInfo.LastName,
                            PhoneNumber = userInfo.PhoneNumber,
                            StreetAddress = userInfo.StreetAddress,
                            City = userInfo.City,
                            Province = userInfo.Province,
                            PostalCode = userInfo.PostalCode,
                            Country = userInfo.Country,
                            JoinDate = userInfo.JoinDate
                        };
                        return View(siteUser);
                    }
                    else
                    {
                        return HttpNotFound("userInfo is null");
                    }

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("User has a null value!");
                }
            }

            return View(siteUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditUserDetails(SiteUserVM formData)
        {
            if (formData != null)
            {
                string userId = formData.Id;
                var userManagerMain = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new SimplyFirstVMContext()));
                using (var db = new SimplyFirstVMContext())
                {
                    var user = db.Users.Where(e => e.Id == userId).FirstOrDefault();
                    //UserInformation userInformation = db.UserInformation.Where(u => u.UserId == user.Id).FirstOrDefault();
                    UserInformation userInfo = db.UserInformation.Where(u => u.UserId == userId).FirstOrDefault();

                    // Updating data from form
                    // AspNetUsers Table
                    user.UserName = formData.UserName;
                    user.Email = formData.Email;
                    user.EmailConfirmed = formData.EmailConfirmed;

                    // UserInformation Table
                    userInfo.FirstName = formData.FirstName;
                    userInfo.LastName = formData.LastName;
                    userInfo.PhoneNumber = formData.PhoneNumber;
                    userInfo.StreetAddress = formData.StreetAddress;
                    userInfo.City = formData.City;
                    userInfo.Province = formData.Province;
                    userInfo.PostalCode = formData.PostalCode;
                    userInfo.Country = formData.Country;
                    // Join Date needs to standardized (Should it be the first time usre signs on or somethign else)
                    userInfo.JoinDate = formData.JoinDate;

                    // Saving changes
                    db.SaveChanges();

                    return RedirectToAction("Index", "Admin");
                }
            }
            

            return HttpNotFound("No form data passed onto EditUserDetails Post Action method.");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UserDetails(string id)
        {
            string userId = id;
            SiteUserVM siteUser = new SiteUserVM();
            var userManagerMain = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new SimplyFirstVMContext()));
            using (var db = new SimplyFirstVMContext())
            {
                var user = db.Users.Where(e => e.Id == id).FirstOrDefault();
                //UserInformation userInformation = db.UserInformation.Where(u => u.UserId == user.Id).FirstOrDefault();
                UserInformation userInfo = db.UserInformation.Where(u => u.UserId == userId).FirstOrDefault();

                if (user != null)
                {
                    if (userInfo != null)
                    {
                        siteUser = new SiteUserVM
                        {
                            Id = userId,
                            UserName = user.UserName,
                            Email = user.Email,
                            EmailConfirmed = user.EmailConfirmed,
                            FirstName = userInfo.FirstName,
                            LastName = userInfo.LastName,
                            PhoneNumber = userInfo.PhoneNumber,
                            StreetAddress = userInfo.StreetAddress,
                            City = userInfo.City,
                            Province = userInfo.Province,
                            PostalCode = userInfo.PostalCode,
                            Country = userInfo.Country,
                            JoinDate = userInfo.JoinDate
                        };
                        return View(siteUser);
                    }
                    else
                    {
                        return HttpNotFound("userInfo is null");
                    }

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("User has a null value!");
                }
            }

            return View(siteUser);
        }
        
        public async Task<ActionResult> DeleteUser(string id)
        {
            var _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new SimplyFirstVMContext()));

            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await _userManager.FindByIdAsync(id);
                var logins = user.Logins;
                var rolesForUser = await _userManager.GetRolesAsync(id);

                using (var transaction = db.Database.BeginTransaction())
                {
                    foreach (var login in logins.ToList())
                    {
                        await _userManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            // item should be the name of the role
                            var result = await _userManager.RemoveFromRoleAsync(user.Id, item);
                        }
                    }

                    await _userManager.DeleteAsync(user);
                    transaction.Commit();
                }

                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

    }
}
