using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Simply_First.Controllers
{   
    [Authorize]
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        // Secure Area
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