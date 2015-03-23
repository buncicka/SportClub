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
using SportClub.ViewModels;

namespace SportClub.Controllers
{
    public class SportsController : Controller
    {
        private SportContext db = new SportContext();

        // GET: Sports
        public ActionResult Index(int? id, int? InstructorID)
        {
            var viewModel = new SportIndexData();

            viewModel.Sports = db.Sports
                .Include(i => i.Instructors)
                .OrderBy(i => i.Name);

            if (id != null)
            {
                ViewBag.SportID = id.Value;
                viewModel.Instructors = viewModel.Sports.Where(
                    i => i.SportID == id.Value).Single().Instructors;
            }

            if (InstructorID != null)
            {
                ViewBag.InstructorID = InstructorID.Value;
                var selectedInstructors = viewModel.Instructors.Where(x => x.ID == InstructorID).Single();
                db.Entry(selectedInstructors).Collection(x => x.Enrollments).Load();
                foreach (Enrollment enrollment in selectedInstructors.Enrollments)
                {
                    db.Entry(enrollment).Reference(x => x.Members).Load();
                }

                viewModel.Enrollments = selectedInstructors.Enrollments;
            }

            return View(viewModel);
        }

        // GET: Sports/AddInstructors
        public ActionResult AddInstructors(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = db.Sports
                .Include(i => i.Instructors)
                .Where(i => i.SportID == id)
                .Single();
            PopulateAssignedInstructorData(sport);
            if (sport == null)
            {
                return HttpNotFound();
            }
            return View(sport);
        }

        // POST: Sports/AddInstructors
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddInstructors(int? id, string[] selectedInstructors)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sportToUpdate = db.Sports
               .Include(i => i.Instructors)
               .Where(i => i.SportID == id)
               .Single();

            if (TryUpdateModel(sportToUpdate, "",
               new string[] { "Name", "Price"}))
            {
                try
                {
                    UpdateSportInstructors(selectedInstructors, sportToUpdate);

                    db.Entry(sportToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedInstructorData(sportToUpdate);
            return View(sportToUpdate);
        }

        private void UpdateSportInstructors(string[] selectedInstructors, Sport sportToUpdate)
        {
            if (selectedInstructors == null)
            {
                sportToUpdate.Instructors = new List<Instructor>();
                return;
            }

            var selectedInstructorsHS = new HashSet<string>(selectedInstructors);
            var sportInstructors = new HashSet<int>
                (sportToUpdate.Instructors.Select(c => c.ID));
            foreach (var instruct in db.Instructors)
            {
                if (selectedInstructorsHS.Contains(instruct.ID.ToString()))
                {
                    if (!sportInstructors.Contains(instruct.ID))
                    {
                        sportToUpdate.Instructors.Add(instruct);
                    }
                }
                else
                {
                    if (sportInstructors.Contains(instruct.ID))
                    {
                        sportToUpdate.Instructors.Remove(instruct);
                    }
                }
            }
        }

        // GET: Sports/Create
        public ActionResult Create()
        {
            var sport = new Sport();
            sport.Instructors = new List<Instructor>();
            PopulateAssignedInstructorData(sport);
            return View();
        }

        // POST: Sports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SportID, Name, StartDate, Price")] Sport sport, string[] selectedInstructors)
        {
            if (selectedInstructors != null)
            {
                sport.Instructors = new List<Instructor>();
                foreach (var instructor in selectedInstructors)
                {
                    var instructorToAdd = db.Instructors.Find(int.Parse(instructor));
                    sport.Instructors.Add(instructorToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                db.Sports.Add(sport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateAssignedInstructorData(sport);
            return View(sport);
        }

        // GET: Sports/Edit
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
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "LastName");
            return View(sport);
        }

        // POST: Sports/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "Name", "StartDate", "Price"};
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
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
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

        // GET: Sports/Delete
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

        // POST: Sports/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            Sport sport = db.Sports.Find(id);
            db.Sports.Remove(sport);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private void PopulateAssignedInstructorData(Sport sport)
        {
            var allInstructors = db.Instructors;
            var sportInstructors = new HashSet<int>(sport.Instructors.Select(c => c.ID));
            var viewModel = new List<AssignedInstructorData>();
            foreach (var instructor in allInstructors)
            {
                viewModel.Add(new AssignedInstructorData
                {
                    ID = instructor.ID,
                    LastName = instructor.LastName,
                    Assigned = sportInstructors.Contains(instructor.ID)
                });
            }
            ViewBag.Instructors = viewModel;
        }

        protected override void Dispose(bool disposing)
        {
            if (db!= null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
