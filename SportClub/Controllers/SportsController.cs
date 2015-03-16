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
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace SportClub.Controllers
{
    public class SportsController : Controller
    {
        private SportContext db = new SportContext();

        // GET: Sports
        public async Task<ActionResult> Index()
        {
            var sports = db.Sports.Include(d => d.Administrator);
            return View(await sports.ToListAsync());  
        }

        // GET: Sports/Details/5
        public async  Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Sport sport =  db.Sports.Find(id);

            string query = "SELECT * FROM Sport WHERE SportID = @p0";
            Sport sport = await db.Sports.SqlQuery(query, id).SingleOrDefaultAsync();

            if (sport == null)
            {
                return HttpNotFound();
            }
            return View(sport);
        }

        // GET: Sports/Create
        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "LastName");
            return View();
        }

        // POST: Sports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SportID, Name, StartDate, Price, InstructorID")] Sport sport)
        {
            
            if (ModelState.IsValid)
            {
                db.Sports.Add(sport);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "LastName", sport.InstructorID);
            return View(sport);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = await db.Sports.FindAsync(id);
            if (sport == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "LastName", sport.InstructorID);
            return View(sport);
        }

        // GET: Sports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "Name", "StartDate", "Price", "InstructorID"};
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sportToUpdate = await db.Sports.FindAsync(id);
            if (sportToUpdate == null)
            {
                Sport deletedSport = new Sport();
                TryUpdateModel(deletedSport, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "LastName", deletedSport.InstructorID);
                return View(deletedSport);
            }

            if (TryUpdateModel(sportToUpdate, fieldsToBind))
            {
                try
                {
                    //db.Entry(sportToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.Entry(sportToUpdate).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Sport)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Sport)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);
                        if (databaseValues.StartDate != clientValues.StartDate)
                            ModelState.AddModelError("StartDate", "Current value: "
                                + String.Format("{0:d}", databaseValues.StartDate));
                        if (databaseValues.Price != clientValues.Price)
                            ModelState.AddModelError("Price", "Current value: "
                                + String.Format("{0:c}", databaseValues.Price));
                        if (databaseValues.InstructorID != clientValues.InstructorID)
                            ModelState.AddModelError("InstructorID", "Current value: "
                                + db.Instructors.Find(databaseValues.InstructorID).LastName);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        //sportToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "LastName", sportToUpdate.InstructorID);
            return View(sportToUpdate);
        }

        // POST: Sports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      
        

        // GET: Sports/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = await db.Sports.FindAsync(id);
            if (sport == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(sport);
        }

        // POST: Sports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            Sport sport = db.Sports.Find(id);
            db.Sports.Remove(sport);
            await db.SaveChangesAsync();
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

        public object SelectedInstructor { get; set; }

        public int? InstructorID { get; set; }
    }
}
