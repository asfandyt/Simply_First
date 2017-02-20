using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Simply_First.Models;

namespace Simply_First.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SimplyFirstVMContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SimplyFirstVMContext context)
        {
            //This method will be called after migrating to the latest version.

            //if (!context.AspNetUsers.Any(r => r.Email == "gbola4@my.bcit.ca"))
            //{
            //    var store = new RoleStore<IdentityRole>(context);
            //    var manager = new RoleManager<IdentityRole>(store);
            //    var role = new IdentityRole { Name = "Admin" };

            //    manager.Create(role);
            //}

            //var manager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new SimplyFirstVMContext()));

            ////var user = new ApplicationUser()
            ////{
            ////    UserName = "SuperPowerUser",
            ////    Email = "taiseer.joudeh@mymail.com",
            ////    EmailConfirmed = true,
            ////    FirstName = "Taiseer",
            ////    LastName = "Joudeh",
            ////    Level = 1,
            ////    JoinDate = DateTime.Now.AddYears(-3)
            ////};

            //var admin = new AspNetUsers()
            //{
            //    Email = "gbola4@my.bcit.ca",
            //    EmailConfirmed = true,
            //};

            ////manager.Create(user, "MySuperP@ssword!");
            //manager.Create(admin, "password");

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SimplyFirstVMContext()));

            //if (roleManager.Roles.Count() == 0)
            //{
            //    roleManager.Create(new IdentityRole { Name = "Admin" });
            //    roleManager.Create(new IdentityRole { Name = "Employee" });
            //    roleManager.Create(new IdentityRole { Name = "User" });
            //}

            ////var adminUser = manager.FindByName("SuperPowerUser");
            //var adminUserGurkirat = manager.FindByName("Gurkirat");

            ////manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });
            //manager.AddToRoles(adminUserGurkirat.Id, new string[] { "Admin" });
        }
    }
}
