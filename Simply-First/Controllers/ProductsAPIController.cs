using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Simply_First.Models;
using System.Web.Http.Cors;

namespace Simply_First.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductsAPIController : ApiController
    {
        private SimplyFirstVMContext db = new SimplyFirstVMContext();

        // GET: api/ProductsAPI
        public IQueryable<Products> GetProducts()
        {
            return db.Products;
        }

        // GET: api/ProductsAPI/5
        [ResponseType(typeof(Products))]
        public IHttpActionResult GetProducts(int id)
        {
<<<<<<< HEAD
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

=======
>>>>>>> e2a7c7f8aad29cd5a52bb7e8c98d4f538ade8757
            Products products = db.Products.Find(id);

            if (products == null)
            {
                return NotFound();
            }
<<<<<<< HEAD

            return View(products);
        }
=======
>>>>>>> e2a7c7f8aad29cd5a52bb7e8c98d4f538ade8757

            return Ok(products);
        }

        // PUT: api/ProductsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProducts(int id, Products products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != products.ProductId)
            {
                return BadRequest();
            }

            db.Entry(products).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProductsAPI
        [ResponseType(typeof(Products))]
        public IHttpActionResult PostProducts(Products products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(products);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = products.ProductId }, products);
        }

        // DELETE: api/ProductsAPI/5
        [ResponseType(typeof(Products))]
        public IHttpActionResult DeleteProducts(int id)
        {
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return NotFound();
            }

            db.Products.Remove(products);
            db.SaveChanges();

            return Ok(products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductsExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}