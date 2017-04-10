using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Simply_First.ViewModels;

namespace Simply_First.Repositories
{
    public class UserRepo
    {
        private SimplyFirstVMContext db = new SimplyFirstVMContext();

        public UserInformation GetAll(string userId)
        {
            return null;
        }
    }
}