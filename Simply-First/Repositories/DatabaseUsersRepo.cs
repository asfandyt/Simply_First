using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Simply_First.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simply_First.Repositories
{
    public class DatabaseUsersRepo
    {
        private SimplyFirstVMContext db = new SimplyFirstVMContext();

        public SiteUserVM GetAll(string UserId)
        {
            SiteUserVM user = db.Users.Where(a => a.Id == UserId)
                                                     .Select(u =>
                                                     new SiteUserVM()
                                                     {
                                                         UserName = u.UserName,
                                                         Email = u.Email,
                                                         EmailConfirmed = u.EmailConfirmed,
                                                         Id = u.Id
                                                     }).FirstOrDefault();

            return user;
        }
    }
}