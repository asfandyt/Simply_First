using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Simply_First.ViewModels
{
    public class SiteUserVM
    {
        public string Id { get; set; }

        [Display(Name ="User Name")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }
        
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class SiteUsersRoleVM
    {
        public string UserId { get; set; }

        [Display(Name = "Role Name")]
        public string RoleId { get; set; }

        [Display(Name = "User Name")]
        public List<IdentityUserRole> Users { get; set; }
    }
}
