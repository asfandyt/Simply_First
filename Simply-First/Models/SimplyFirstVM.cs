using System;
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
    public class Products
    {
        [Key]
        [Required]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        [Required]
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name="Image")]
        public string ProductImage { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "A value bigger than 0 is needed.")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
    }

    public class Address
    {
        [Key]
        [Required]
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Province")]
        public string Province { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Range(0, 10, ErrorMessage = "A value bigger than 0 is needed.")]
        public int PhoneNumber { get; set; }
    }

    public class SimplyFirstVMContext : IdentityDbContext<IdentityUser>
    {
        public SimplyFirstVMContext() : base("DefaultConnection") { }

        public DbSet<Products> Products { get; set; }
        public DbSet<Address> Address { get; set; }

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