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

    public class Address : IdentityUser
    {
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
        [Display(Name = "Date Joined")]
        public DateTime JoinDate { get; set; }
    }


    public class ShoppingCart
    {
        #region Properties
        [Key]
        [Required]
        public int ShoppingCartId { get; set; }
        public List<CartItem> Items { get; private set; }

        #endregion

        #region Singleton Implementation

        // Readonly properties can only be set in initialization or in a constructor
        public static readonly ShoppingCart Instance;

        // The static constructor is called as soon as the class is loaded into memory
        static ShoppingCart()
        {
            // If the cart is not in the session, create one and put it there
            // Otherwise, get it from the session
            if (HttpContext.Current.Session["ASPNETShoppingCart"] == null)
            {
                Instance = new ShoppingCart();
                Instance.Items = new List<CartItem>();
                HttpContext.Current.Session["ASPNETShoppingCart"] = Instance;
            }
            else
            {
                Instance = (ShoppingCart)HttpContext.Current.Session["ASPNETShoppingCart"];
            }
        }

        // A protected constructor ensures that an object can't be created from outside
        protected ShoppingCart() { }

        #endregion

        #region Item Modification Methods
        /**
         * AddItem() - Adds an item to the shopping 
         */
        public void AddItem(int productId)
        {
            // Create a new item to add to the cart
            CartItem newItem = new CartItem(productId);

            // If this item already exists in our list of items, increase the quantity
            // Otherwise, add the new item to the list
            if (Items.Contains(newItem))
            {
                foreach (CartItem item in Items)
                {
                    if (item.Equals(newItem))
                    {
                        item.Quantity++;
                        return;
                    }
                }
            }
            else
            {
                newItem.Quantity = 1;
                Items.Add(newItem);
            }
        }

        /**
         * SetItemQuantity() - Changes the quantity of an item in the cart
         */
        public void SetItemQuantity(int productId, int quantity)
        {
            // If we are setting the quantity to 0, remove the item entirely
            if (quantity == 0)
            {
                RemoveItem(productId);
                return;
            }

            // Find the item and update the quantity
            CartItem updatedItem = new CartItem(productId);

            foreach (CartItem item in Items)
            {
                if (item.Equals(updatedItem))
                {
                    item.Quantity = quantity;
                    return;
                }
            }
        }

        /**
         * RemoveItem() - Removes an item from the shopping cart
         */
        public void RemoveItem(int productId)
        {
            CartItem removedItem = new CartItem(productId);
            Items.Remove(removedItem);
        }
        #endregion

        #region Reporting Methods
        /**
         * GetSubTotal() - returns the total price of all of the items
         *                 before tax, shipping, etc.
         */
        public decimal GetSubTotal()
        {
            decimal subTotal = 0;
            foreach (CartItem item in Items)
                subTotal += item.TotalPrice;

            return subTotal;
        }
        #endregion
    }

    public class CartItem : IEquatable<CartItem>
    {
        #region Properties
        private SimplyFirstVMContext db = new SimplyFirstVMContext();
        // A place to store the quantity in the cart
        // This property has an implicit getter and setter.
        public int Quantity { get; set; }

        private int _productId;
        [Key]
        [Required]
        public int ProductId
        {
            get { return _productId; }
            set
            {
                // To ensure that the Prod object will be re-created
                _product = null;
                _productId = value;
            }
        }
        private Products _product = null;
        public Products Prod
        {
            get
            {
                // Lazy initialization - the object won't be created until it is needed
                if (_product == null)
                {
                    ////make query using product id////////////////////
                    _product = db.Products.Where(e => e.ProductId == ProductId).FirstOrDefault();
                }

                return _product;
            }
        }

        public string Description
        {
            get { return Prod.ProductDescription; }
        }

        public decimal UnitPrice
        {
            get { return Prod.Price; }
        }

        public decimal TotalPrice
        {
            get { return UnitPrice * Quantity; }
        }

        #endregion

        // CartItem constructor just needs a productId
        public CartItem(int productId)
        {
            this.ProductId = productId;
        }

        /**
         * Equals() - Needed to implement the IEquatable interface
         *    Tests whether or not this item is equal to the parameter
         *    This method is called by the Contains() method in the List class
         *    We used this Contains() method in the ShoppingCart AddItem() method
         */
        public bool Equals(CartItem item)
        {
            return item.ProductId == this.ProductId;
        }
    }

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

        public DbSet<Products> Products { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
    }
}