using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Simply_First.Models
{

    public class AspNetRoles
    {
        public AspNetRoles()
        {
            this.AspNetUsers = new HashSet<AspNetUsers>();
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }

    public class AspNetUsers
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        //public string UserName { get; set; }

        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetRoles> AspNetRoles { get; set; }
    }

    public class AspNetUserClaims
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
    }

    public class AspNetUserLogins
    {
        [Key]
        [Column(Order = 1)]
        public string LoginProvider { get; set; }
        [Key]
        [Column(Order = 2)]
        public string ProviderKey { get; set; }
        [Key]
        [Column(Order = 3)]
        public string UserId { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
    }

    public class AspNetUserRoles
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string RoleId { get; set; }
        //public AspNetUser AspNetUser { get; set; }
        //public AspNetRole AspNetRole { get; set; }

        //public AspNetUserRoles(AspNetUser user, AspNetRole role) {
        //    AspNetUser = user;
        //    AspNetRole = role;
        //    RoleId = role.Id;
        //    UserId = user.Id;
        //}

    }

    public class SimplyFirstVMContext : DbContext
    {
        public SimplyFirstVMContext() : base("DefaultConnection") { }

        // This method overrides some framework default behaviour.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Prevent the framework from pluralizing table names
            // since the actual database table names are singular.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //// Prevent the framework from trying to auto-generate a primary key
            //// for Student since the Student table does not use IDENTITY.
            //modelBuilder.Configurations.Add(new StudentConfiguration());  
        }

        public DbSet<AspNetRoles> AspNetRoles { get; set; }
        public DbSet<AspNetUsers> AspNetUsers { get; set; }
        public DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}