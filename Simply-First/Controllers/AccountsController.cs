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
    public class AccountsController : Controller
    {
        // ReSharper disable once InconsistentNaming
        const string EMAIL_CONFIRMATION = "EmailConfirmation";
        // ReSharper disable once InconsistentNaming
        const string PASSWORD_RESET = "ResetPassword";

        void CreateTokenProvider(UserManager<IdentityUser> manager, string tokenType)
        {
            manager.UserTokenProvider = new EmailTokenProvider<IdentityUser>();
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

            AccountRepo accountRepo = new AccountRepo();

            if (ModelState.IsValid)
            {
                string loginError;

                if (accountRepo.ValidLogin(login, out loginError))
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;

                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, login.Email),
                    },
                    DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);

                    // SignIn() accepts ClaimsIdentity and issues logged in cookie. 
                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);

                    return RedirectToAction("SecureArea", "Accounts");
                }

                TempData["LoginError"] = "Invalid email or password!";
            }

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisteredUserVM newUser)
        {
            SimplyFirstVMContext context = new SimplyFirstVMContext();

            var checkEmail = context.Users.Where(e => e.Email == newUser.Email).FirstOrDefault();

            if (checkEmail != null)
            {
                TempData["RegisterError"] = "The email address is already in use.";
            }

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var identityUser = new IdentityUser()
            {
                UserName = newUser.Email,
                Email = newUser.Email
            };

            IdentityResult result = manager.Create(identityUser, newUser.Password);

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
                if (result.Succeeded)
                {
                    CreateTokenProvider(manager, EMAIL_CONFIRMATION);

                    var code = manager.GenerateEmailConfirmationToken(identityUser.Id);

                    var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = identityUser.Id, code = code },
                                                                               protocol: Request.Url.Scheme);

                    EmailRepo emailRepo = new EmailRepo();
                    string email = "Please confirm your email address \n You have created a Simply First ID" + "\n" + callbackUrl;
                    string subject = "Please confirm email for your Simply First ID";

                    emailRepo.SendEmail(newUser.Email, subject, email);

                    // DOES NOT WORK

                    //using (var db = new SimplyFirstVMContext())
                    //{
                    //    var newUserRole = new UserRoleVM();

                    //    IdentityUser user = db.Users.Where(e => e.Email == newUserRole.Email).FirstOrDefault();
                    //    //var role = db.Roles.Where(r => r.Name == newUserRole.RoleName).FirstOrDefault();

                    //    var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new SimplyFirstVMContext()));

                    //    if (user != null)
                    //    {
                    //        userManager.AddToRoles(user.Id, "User");
                    //    }                      
                    //}

                    TempData["RegisterSuccess"] = "Registered successfully, check your email for confirmation link!";

                    return RedirectToAction("Register");

                    //ViewBag.FakeConfirmation = email;
                }

                TempData["RegisterError"] = result.Errors.FirstOrDefault().ToString();
            }

            return View();
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();

            TempData["Logout"] = "You've successfully logged out!";
            return RedirectToAction("Index", "Accounts");
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
                {
                    ViewBag.Message = "You are now registered!";
                }
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
                    var callbackUrl = Url.Action("ResetPassword", "Accounts", new { userId = user.Id, code = code },
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

            ViewBag.Result = result.Succeeded ? "The password has been reset." : "The password has not been reset.";

            return View();
        }

        [Authorize]
        public ActionResult SecureArea()
        {
            Page_Load();
            return View();
        }

        protected void Page_Load()
        {
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetNoStore();
        }
    }
}