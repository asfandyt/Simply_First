using Simply_First.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Simply_First.Controllers
{   
    public class UsersController : Controller
    {
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