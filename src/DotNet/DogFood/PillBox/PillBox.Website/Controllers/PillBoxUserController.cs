using PillBox.DAL.Entities;
using PillBox.Model.Entities;
using PillBox.Services;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using PillBox.Website.Models;
using System.Linq;

namespace PillBox.Website.Controllers
{
    [Authorize]
    public class PillBoxUserController : Controller
    {
        private PillBoxDbContext db;

        public PillBoxUserController()
        {
            db = new PillBoxDbContext();
        }

        //
        // GET: /Patient/

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            PillBoxUser user = db.Set<PillBoxUser>()
                                .Include("Medicines")
                                .Include("Reminders")
                                .FirstOrDefault(u => u.Id == userId);

            var model = new PillBoxUserViewModel(user);

            return View(model);
        }

        //
        // GET: /Patient/Details/5

        public async Task<ActionResult> Details(string id)
        {
            PillBoxUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        ////
        //// GET: /Patient/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Patient/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(PillBoxUser patient)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Patients.Add(patient);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(patient);
        //}

        ////
        //// GET: /Patient/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    PillBoxUser patient = db.Patients.Find(id);
        //    if (patient == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(patient);
        //}

        ////
        //// POST: /Patient/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(PillBoxUser patient)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(patient).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(patient);
        //}

        ////
        //// GET: /Patient/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    PillBoxUser patient = db.Patients.Find(id);
        //    if (patient == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(patient);
        //}

        ////
        //// POST: /Patient/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    PillBoxUser patient = db.Patients.Find(id);
        //    db.Patients.Remove(patient);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //public ActionResult WeeklySummary(int id = 0)
        //{
        //    PillBoxUser patient = db.Patients.Find(id);
        //    if (patient == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(patient);
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private PillBoxUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<PillBoxUserManager>();
            }
        }

    }
}
