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
using System.Data.Entity.Infrastructure;

namespace SportClub.Controllers
{
    public class GroupsController : Controller
    {
        private SportContext db = new SportContext();

        // GET: Groups
        public ActionResult Index(int? SelectedSport)
        {
            var sports = db.Sports.OrderBy(q => q.Name).ToList();
            ViewBag.SelectedSport = new SelectList(sports, "SportID", "Name", SelectedSport);
            int sportID = SelectedSport.GetValueOrDefault();

            IQueryable<Group> groups = db.Groups
                .Where(c => !SelectedSport.HasValue || c.SportID == sportID)
                .OrderBy(d => d.GroupID)
                .Include(d => d.Sport);
            var sql = groups.ToString();
            return View(groups.ToList());
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            PopulateSportDropDownList();
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupID,Title, SportID")] Group group)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Groups.Add(group);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateSportDropDownList(group.SportID);
            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            PopulateSportDropDownList(group.SportID);
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var groupToUpdate = db.Groups.Find(id);
            if (TryUpdateModel(groupToUpdate, "",
               new string[] { "Title", "SportID" }))
            {
                try
                {
                    db.Entry(groupToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateSportDropDownList(groupToUpdate.SportID);
            return View(groupToUpdate);
        }

        private void PopulateSportDropDownList(object selectedSport = null)
        {
            var departmentsQuery = from d in db.Sports
                                   orderby d.Name
                                   select d;

            ViewBag.SportID = new SelectList(departmentsQuery, "SportID", "Name", selectedSport);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }

            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
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
