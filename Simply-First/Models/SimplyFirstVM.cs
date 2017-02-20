﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Simply_First.Models
{
    public class SimplyFirstVMContext : IdentityDbContext<IdentityUser>
    {
        public SimplyFirstVMContext() : base("DefaultConnection") { }

        // This method overrides some framework default behaviour.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Prevent the framework from pluralizing table names
            // since the actual database table names are singular.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Prevent the framework from trying to auto-generate a primary key
            // for Student since the Student table does not use IDENTITY.
            // modelBuilder.Configurations.Add(new StudentConfiguration());  

            base.OnModelCreating(modelBuilder);
        }

        public static SimplyFirstVMContext Create()
        {
            return new SimplyFirstVMContext();
        }
    }
}