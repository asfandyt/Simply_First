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

namespace Simply_First
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //RoleVM roleVM = new RoleVM();

            //using (var db = new SimplyFirstVMContext())
            //{
            //    AspNetRoles adminRole = new AspNetRoles();
            //    adminRole.Id = "Admin";
            //    adminRole.Name = "Admin";

            //    if (!db.AspNetRoles.Any(a => a.Id == "Admin"))
            //    {
            //        db.AspNetRoles.Add(adminRole);
            //        db.SaveChanges();
            //    }

            //    AspNetRoles employeeRole = new AspNetRoles();
            //    employeeRole.Id = "Employee";
            //    employeeRole.Name = "Employee";

            //    if (!db.AspNetRoles.Any(a => a.Id == "Employee"))
            //    {
            //        db.AspNetRoles.Add(employeeRole);
            //        db.SaveChanges();
            //    }

            //    AspNetRoles distributor = new AspNetRoles();
            //    distributor.Id = "Distributor";
            //    distributor.Name = "Distributor";

            //    if (!db.AspNetRoles.Any(a => a.Id == "Distributor"))
            //    {
            //        db.AspNetRoles.Add(distributor);
            //        db.SaveChanges();
            //    }

            //    AspNetRoles standardRole = new AspNetRoles();
            //    standardRole.Id = "Standard";
            //    standardRole.Name = "Standard";

            //    if (!db.AspNetRoles.Any(a => a.Id == "Standard"))
            //    {
            //        db.AspNetRoles.Add(standardRole);
            //        db.SaveChanges();
            //    }
            //}
        }

        //void Application_PostAuthenticateRequest()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var name = User.Identity.Name; // Get current user name.

        //        UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
        //        UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
        //        IdentityUser identityUser = manager.FindByName(User.Identity.GetUserName());

        //        System.Web.HttpContext.Current.User.Identity.GetUserId();
        //        if (identityUser != null)
        //        {
        //            var roleQuery = identityUser.Roles.Where(u => u.UserId == identityUser.Id).ToList();

        //            string[] roles = new string[roleQuery.Count];

        //            for (int i = 0; i < roleQuery.Count; i++)
        //            {
        //                roles[i] = roleQuery[i].RoleId;
        //            }

        //            HttpContext.Current.User = Thread.CurrentPrincipal = new GenericPrincipal(User.Identity, roles);
        //        }
        //    }
        //}
    }
}
