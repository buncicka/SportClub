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
using PagedList;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace SportClub.Controllers
{
    public class MembersController : Controller
    {
        private SportContext db = new SportContext();

        // GET: Members
        [HttpGet]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "Lastname_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var members = from s in db.MembersDb
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                members = members.Where(s => s.FirstName.Contains(searchString)
                                       || s.LastName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    members = members.OrderByDescending(s => s.FirstName);
                    break;
                case "Lastname_desc":
                    members = members.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    members = members.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    members = members.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    members = members.OrderBy(s => s.FirstName);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(members.ToPagedList(pageNumber, pageSize));
        }

        // GET: Members/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string query = "SELECT * FROM Members WHERE ID = @p0";
            Members members = await db.MembersDb.SqlQuery(query, id).SingleOrDefaultAsync();
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "Name");
            ViewBag.GroupID = new SelectList(db.Groups, "GroupID", "Title");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,DateOfBirth,SportID,GroupID,EnrollmentDate")] Members members)
        {
            if (ModelState.IsValid)
            {
                db.MembersDb.Add(members);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "Name", members.SportID);
            ViewBag.GroupID = new SelectList(db.Groups, "GroupID", "Title", members.GroupID);
            return View(members);
        }

        // GET: Members/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = await db.MembersDb.FindAsync(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "Name", members.SportID);
            ViewBag.GroupID = new SelectList(db.Groups, "GroupID", "Title", members.GroupID);
            return View(members);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "FirstName", "LastName", "DateOfBirth", "SportID", "GroupID", "EnrollmentDate" };
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var memberToUpdate = await db.MembersDb.FindAsync(id);
            if (memberToUpdate == null)
            {
                Members deletedMember = new Members();
                TryUpdateModel(deletedMember, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                ViewBag.SportID = new SelectList(db.Sports, "SportID", "Name", deletedMember.SportID);
                ViewBag.GroupID = new SelectList(db.Groups, "GroupID", "Title", deletedMember.GroupID);
                return View(deletedMember);
            }

            if (TryUpdateModel(memberToUpdate, fieldsToBind))
            {
                try
                {
                    //db.Entry(sportToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.Entry(memberToUpdate).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Members)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Members)databaseEntry.ToObject();

                        if (databaseValues.FirstName != clientValues.FirstName)
                            ModelState.AddModelError("FirstName", "Current value: "
                                + databaseValues.FirstName);
                        if (databaseValues.LastName != clientValues.LastName)
                            ModelState.AddModelError("LastName", "Current value: "
                                + databaseValues.LastName);
                        if (databaseValues.DateOfBirth != clientValues.DateOfBirth)
                            ModelState.AddModelError("DateOfBird", "Current value: "
                                + String.Format("{0:c}", databaseValues.DateOfBirth));
                        if (databaseValues.SportID != clientValues.SportID)
                            ModelState.AddModelError("SportID", "Current value: "
                                + db.Sports.Find(databaseValues.SportID).Name);
                        if (databaseValues.GroupID != clientValues.GroupID)
                            ModelState.AddModelError("GroupID", "Current value: "
                                + db.Groups.Find(databaseValues.GroupID).Title);
                        if (databaseValues.EnrollmentDate != clientValues.EnrollmentDate)
                            ModelState.AddModelError("EnrollmentDate", "Current value: "
                                + String.Format("{0:d}", databaseValues.EnrollmentDate));
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
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "Name", memberToUpdate.SportID);
            ViewBag.GroupID = new SelectList(db.Groups, "GroupID", "Title", memberToUpdate.GroupID);
            return View(memberToUpdate);


            //[Bind(Include = "ID,FirstName,LastName,DateOfBirth,Sport,Group,EnrollmentDate")] Members members
            /*if (ModelState.IsValid)
            {
                db.Entry(members).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(members);*/
        }

        // GET: Members/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members =await db.MembersDb.FindAsync(id);
            if (members == null)
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
            return View(members);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Members members)
        {
            try
            {
                db.Entry(members).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = members.ID });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(members);
            }
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
