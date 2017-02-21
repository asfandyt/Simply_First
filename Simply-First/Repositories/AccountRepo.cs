using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Simply_First.Models;

namespace Simply_First.Repositories
{
    public class AccountRepo
    {
        public bool ValidLogin(LoginVM login, out string loginError)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };

            var user = userManager.FindByName(login.Email);

            loginError = "";

            if (user == null)
            {
                loginError = "Invalid email or password!";
                return false;
            }

            // User is locked out.
            if (userManager.SupportsUserLockout && userManager.IsLockedOut(user.Id))
            {
                return false;
            }

            // Validated user was locked out but now can be reset.
            if (userManager.CheckPassword(user, login.Password) && userManager.IsEmailConfirmed(user.Id))
            {
                if (userManager.SupportsUserLockout && userManager.GetAccessFailedCount(user.Id) > 0)
                {
                    userManager.ResetAccessFailedCount(user.Id);
                }
            }
            // Login is invalid so increment failed attempts.
            else
            {
                bool lockoutEnabled = userManager.GetLockoutEnabled(user.Id);

                if (userManager.SupportsUserLockout && userManager.GetLockoutEnabled(user.Id))
                {
                    loginError = "Invalid email or password!";
                    userManager.AccessFailed(user.Id);
                    //return false;
                }
                else if (!userManager.SupportsUserLockout && userManager.GetLockoutEnabled(user.Id))
                {
                    loginError = "Invalid email or password!";
                }
                else
                {
                    loginError = "Unexpected Error, please try again!";
                }

                return false;
            }

            return true;
        }
    }
}