using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Simply_First.Models;

namespace Simply_First.Controllers
{
    public class UserInformationsController : Controller
    {
        private SimplyFirstVMContext db = new SimplyFirstVMContext();

        // GET: UserInformations
        public ActionResult Index()
        {
            return View(db.UserInformation.ToList());
        }

        // GET: UserInformations/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInformation userInformation = db.UserInformation.Find(id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            return View(userInformation);
        }

        // GET: UserInformations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,Email,StreetAddress,City,Province,PostalCode,Country,JoinDate")] UserInformation userInformation)
        {
            if (ModelState.IsValid)
            {
                db.UserInformation.Add(userInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userInformation);
        }

        // GET: UserInformations/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInformation userInformation = db.UserInformation.Find(id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            return View(userInformation);
        }

        // POST: UserInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,Email,StreetAddress,City,Province,PostalCode,Country,JoinDate")] UserInformation userInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userInformation);
        }

        // GET: UserInformations/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInformation userInformation = db.UserInformation.Find(id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            return View(userInformation);
        }

        // POST: UserInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserInformation userInformation = db.UserInformation.Find(id);
            db.UserInformation.Remove(userInformation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
