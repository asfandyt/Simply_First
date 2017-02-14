using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Simply_First.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Simply_First.Services;
using Simply_First.Repositories;


namespace Simply_First.Controllers
{
    public class HomeController : Controller
    {
        const string EMAIL_CONFIRMATION = "EmailConfirmation";
        const string PASSWORD_RESET = "ResetPassword";

        void CreateTokenProvider(UserManager<IdentityUser> manager, string tokenType)
        {
            manager.UserTokenProvider = new EmailTokenProvider<IdentityUser>();
        }

        [Authorize]
        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddRole(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                // *** New: Connect to AspNetRole using code first.
                using (var db = new SimplyFirstVMContext())
                {
                    AspNetRoles role = new AspNetRoles();
                    role.Id = roleVM.RoleName;
                    role.Name = roleVM.RoleName;
                    db.AspNetRoles.Add(role);

                    db.SaveChanges();
                }
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddUserToRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddUserToRole(UserRoleVM userRoleVM)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SimplyFirstVMContext())
                {
                    AspNetUsers user = db.AspNetUsers.Where(e => e.Email == userRoleVM.Email).FirstOrDefault();
                    AspNetRoles role = db.AspNetRoles.Where(r => r.Name == userRoleVM.RoleName).FirstOrDefault();

                    AspNetUserRoles userRole = new AspNetUserRoles();
                    userRole.RoleId = role.Id;
                    userRole.UserId = user.Id;

                    db.AspNetUserRoles.Add(userRole);

                    db.SaveChanges();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginVM login)
        {
            // UserStore and UserManager manages data retreival.
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.Find(login.Email, login.Password);

            if (ModelState.IsValid)
            {
                if (ValidLogin(login))
                {
                    IAuthenticationManager authenticationManager
                                           = HttpContext.GetOwinContext()
                                            .Authentication;
                    authenticationManager
                   .SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var identity = new ClaimsIdentity(new[] 
                    {
                        new Claim(ClaimTypes.Name, login.Email),
                    },
                    DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);

                    // SignIn() accepts ClaimsIdentity and issues logged in cookie. 
                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    },  identity);

                    return RedirectToAction("SecureArea", "Home");
                }

                TempData["LoginError"] = "Invalid email or password!";
            }

            return View();
        }

        bool ValidLogin(LoginVM login)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();

            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };

            var user = userManager.FindByName(login.Email);

            if (user == null)
            {
                TempData["LoginError"] = "Invalid email or password!";
                return false;
            }

            // User is locked out.
            if (userManager.SupportsUserLockout && userManager.IsLockedOut(user.Id))
            {
                return false;
            }

            // Validated user was locked out but now can be reset.
            if (userManager.CheckPassword(user, login.Password) && userManager.IsEmailConfirmed(user.Id))
            {
                if (userManager.SupportsUserLockout && userManager.GetAccessFailedCount(user.Id) > 0)
                {
                    userManager.ResetAccessFailedCount(user.Id);
                }
            }
            // Login is invalid so increment failed attempts.
            else
            {
                bool lockoutEnabled = userManager.GetLockoutEnabled(user.Id);

                if (userManager.SupportsUserLockout && userManager.GetLockoutEnabled(user.Id))
                {
                    userManager.AccessFailed(user.Id);
                    return false;
                }
            }
            return true;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisteredUserVM newUser, UserRoleVM newUserRole, RoleVM newRole)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var identityUser = new IdentityUser()
            {
                //UserName = newUser.UserName,
                UserName = newUser.Email,
                Email = newUser.Email
            };

            IdentityResult result = manager.Create(identityUser, newUser.Password);

            // Google Captcha Helper
            CaptchaHelper captchaHelper = new CaptchaHelper();
            string captchaResponse = captchaHelper.CheckRecaptcha();
            ViewBag.CaptchaResponse = captchaResponse;

            //if (result.Succeeded)
            //{
            //    var authenticationManager = HttpContext.Request.GetOwinContext().Authentication;
            //    var userIdentity = manager.CreateIdentity(identityUser, DefaultAuthenticationTypes.ApplicationCookie);
            //    authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
            //}
            if (!captchaResponse.Equals("Valid"))
            {
                TempData["CaptchaError"] = "Please fill out all the form fields correctly!";
            }
            else
            {
                if (result.Succeeded)
                {
                    CreateTokenProvider(manager, EMAIL_CONFIRMATION);

                    var code = manager.GenerateEmailConfirmationToken(identityUser.Id);

                    using (var db = new SimplyFirstVMContext())
                    {
                        AspNetUsers user = db.AspNetUsers.Where(e => e.Email == newUserRole.Email).FirstOrDefault();
                        AspNetRoles role = db.AspNetRoles.Where(r => r.Name == newUserRole.RoleName).FirstOrDefault();

                        AspNetUserRoles userRole = new AspNetUserRoles();
                        userRole.RoleId = "Standard";
                        userRole.UserId = user.Id;

                        db.AspNetUserRoles.Add(userRole);

                        db.SaveChanges();
                    }

                    var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = identityUser.Id, code = code },
                                                                               protocol: Request.Url.Scheme);

                    EmailRepo emailRepo = new EmailRepo();
                    string email = "Please confirm your email address \n You have created a Simply First ID" +  "\n" + callbackUrl;
                    string subject = "Please confirm email for your Simply First ID";

                    emailRepo.SendEmail(newUser.Email, subject, email);

                    //ViewBag.FakeConfirmation = email;
                    TempData["RegisterSuccess"] = "Registered successfully, check your email for confirmation link!";
                    return RedirectToAction("Register");
                }

                TempData["RegisterError"] = ((string[])result.Errors)[0].ToString();
            }

            return View();
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();

            TempData["Logout"] = "You've successfully logged out!";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmEmail(string userId, string code)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);

            var user = manager.FindById(userId);
            CreateTokenProvider(manager, EMAIL_CONFIRMATION);

            try
            {
                IdentityResult result = manager.ConfirmEmail(userId, code);

                if (result.Succeeded)
                    ViewBag.Message = "You are now registered!";
            }
            catch
            {
                ViewBag.Message = "Validation attempt failed!";
            }

            return View();
        }

        /**
        *
        * This will enable password resets for users
        *
        */
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var userStore = new UserStore<IdentityUser>();

            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindByEmail(email);

            // Google Captcha Helper
            CaptchaHelper captchaHelper = new CaptchaHelper();
            string captchaResponse = captchaHelper.CheckRecaptcha();
            ViewBag.CaptchaResponse = captchaResponse;

            if (!captchaResponse.Equals("Valid"))
            {
                TempData["CaptchaError"] = "Please fill out all the form fields correctly!";
            }
            else
            {
                if (user != null)
                {
                    CreateTokenProvider(manager, PASSWORD_RESET);

                    var code = manager.GeneratePasswordResetToken(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Home", new { userId = user.Id, code = code },
                                                                                protocol: Request.Url.Scheme);

                    EmailRepo emailRepo = new EmailRepo();
                    string sendEmail = "Please reset your password by clicking \n" + callbackUrl;
                    string subject = "Link to reset your Simply First account password";

                    emailRepo.SendEmail(user.Email, subject, sendEmail);

                    TempData["ForgotPasswordSuccess"] = "Please check your email for password reset link!";

                    return RedirectToAction("ForgotPassword");
                }
                else
                {
                    TempData["ForgotPasswordError"] = "Cannot find the user with email " + user.Email + "!";
                }
            }

            return View("ForgotPassword");
        }

        [HttpGet]
        public ActionResult ResetPassword(string userId, string code)
        {
            ViewBag.PasswordToken = code;
            ViewBag.UserID = userId;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string password, string passwordConfirm, string passwordToken, string userId)
        {

            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindById(userId);
            CreateTokenProvider(manager, PASSWORD_RESET);

            IdentityResult result = manager.ResetPassword(userId, passwordToken, password);

            if (result.Succeeded)
            {
                ViewBag.Result = "The password has been reset.";
            }
            else
            {
                ViewBag.Result = "The password has not been reset.";
            }

            return View();
        }

        [Authorize]
        public ActionResult SecureArea()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // To allow more than one role access use syntax like the following:
        // [Authorize(Roles="Admin, Staff")]
        public ActionResult AdminOnly()
        {
            return View();
        }
    }
}