using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Simply_First.Models;
using System.Web.Http;
using Simply_First.App_Start;

namespace Simply_First
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
            .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_PostAuthenticateRequest()
        {
            if (User.Identity.IsAuthenticated)
            {
                var name = User.Identity.Name; // Get current user name.

                UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
                IdentityUser identityUser = manager.FindByName(User.Identity.GetUserName());

                if (identityUser != null)
                {
                    var db = new SimplyFirstVMContext();
                    var roleQuery = identityUser.Roles.Where(u => u.UserId == identityUser.Id).ToList();
                    var roles = db.Roles.ToList();
                    string[] roleNames = new string[roleQuery.Count()];

                    for (int i = 0; i < roleQuery.Count; i++)
                    {
                        IdentityRole usrRole = roles.Where(r => r.Id == roleQuery[i].RoleId).FirstOrDefault();
                        roleNames[i] = usrRole.Name;
                    }

                    HttpContext.Current.User = Thread.CurrentPrincipal =
                                               new GenericPrincipal(User.Identity, roleNames);
                }
            }
        }
    }
}
