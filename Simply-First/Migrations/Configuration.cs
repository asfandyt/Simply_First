using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Simply_First.Models;

namespace Simply_First.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SimplyFirstVMContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SimplyFirstVMContext context)
        {
            // This method will be called after migrating to the latest version.

            // Create a user on start
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new SimplyFirstVMContext()));

            var admin = new IdentityUser()
            {
                UserName = "gbola4@my.bcit.ca",
                Email = "gbola4@my.bcit.ca",
                EmailConfirmed = true,
            };

            var admin_two = new IdentityUser()
            {
                UserName = "ccheung120@my.bcit.ca",
                Email = "ccheung120@my.bcit.ca",
                EmailConfirmed = true,
            };

            var admin_three = new IdentityUser()
            {
                UserName = "admin@sf.com",
                Email = "admin@sf.com",
                EmailConfirmed = true,
            };

            var admin_four = new IdentityUser()
            {
                UserName = "game.design.ty@gmail.com",
                Email = "game.design.ty@gmail.com",
                EmailConfirmed = true,
            };

            // Assign user password on start
            userManager.Create(admin, "password");
            userManager.Create(admin_two, "password");
            userManager.Create(admin_three, "password");
            userManager.Create(admin_four, "password");
            // adding products

            var p1 = new Products()
            {
                ProductId = 1,
                ProductName = "Mavic Pro",
                ProductDescription = "The best light weight drone tech",
                Manufacturer = "DJI",
                Quantity = 99,
                ProductImage = "11",
                Price = 799.99M
            };
            var p2 = new Products()
            {
                ProductId = 2,
                ProductName = "Phantom 4",
                ProductDescription = "DJI staple products",
                Manufacturer = "DJI",
                Quantity = 29,
                ProductImage = "22",
                Price = 629.99M
            };
            var p3 = new Products()
            {
                ProductId = 3,
                ProductName = "Inspire II",
                ProductDescription = "The best professional drone on the market",
                Manufacturer = "DJI",
                Quantity = 39,
                ProductImage = "33",
                Price = 3999.99M
            };

            // Assign user password on start
            context.Products.Add(p1);
            context.Products.Add(p2);
            context.Products.Add(p3);


            // Create roles on start
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SimplyFirstVMContext()));

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "Employee" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            // Assign User admin on start
            var adminUserGurkirat = userManager.FindByName("gbola4@my.bcit.ca");
            userManager.AddToRoles(adminUserGurkirat.Id, new string[] { "Admin" });

            var adminUserCusson = userManager.FindByName("ccheung120@my.bcit.ca");
            userManager.AddToRoles(adminUserCusson.Id, new string[] { "Admin" });

            var adminUser = userManager.FindByName("admin@sf.com");
            userManager.AddToRoles(adminUser.Id, new string[] { "Admin" });

            var adminUserTy = userManager.FindByName("game.design.ty@gmail.com");
            userManager.AddToRoles(adminUserTy.Id, new string[] { "Admin" });
        }
    }
}
