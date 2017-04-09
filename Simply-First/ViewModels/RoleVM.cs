using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Simply_First.ViewModels
{
    public class RoleVM
    {
        [Required]
        [Display(Name = "User Role")]
        public string RoleName { get; set; }
    }
}