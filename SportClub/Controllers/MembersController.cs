using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SportClub.DAL;
using SportClub.Models;

namespace SportClub.Controllers
{
    public class MembersController : Controller
    {
        private SportContext db = new SportContext();

        // GET: Members
        [HttpGet]
        public ActionResult Index()
        {
            
            return View(db.MembersDb.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.MembersDb.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // GET: Members/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,DateOfBirth,Sport,Group,EnrollmentDate")] Members members)
        {
            if (ModelState.IsValid)
            {
                db.MembersDb.Add(members);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(members);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.MembersDb.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,DateOfBirth,Sport,Group,EnrollmentDate")] Members members)
        {
            if (ModelState.IsValid)
            {
                db.Entry(members).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(members);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.MembersDb.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Members members = db.MembersDb.Find(id);
            db.MembersDb.Remove(members);
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
