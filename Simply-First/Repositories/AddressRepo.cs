using Simply_First.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simply_First.Repositories
{
    public class AddressRepo
    {
        private SimplyFirstVMContext db = new SimplyFirstVMContext();

        //public SiteUserVM GetAll(string email)
        //{
        //    SiteUserVM user = db.Users.Where(a => a.Email == email)
        //                                             .Select(u =>
        //                                             new Address()
        //                                             {
        //                                                 id = u.UserName,
        //                                                 Email = u.Email,
        //                                                 EmailConfirmed = u.EmailConfirmed,
        //                                                 Id = u.Id
        //                                             }).FirstOrDefault();

        //    return user;
        //}
    }
}