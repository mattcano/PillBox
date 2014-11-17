using System.Data.Entity;
using System.Web.Mvc;
using PillBox.Model.Entities;
using PillBox.DAL.Entities;
using System.Linq;

namespace MvcApplication2.Controllers
{
    public class UserMedicineMapController : Controller
    {
        private PillBoxContext db = new PillBoxContext();

        //
        // GET: /UserMedicineMap/

        public ActionResult Index()
        {
            var usermedicinemaps = db.UserMedicineMaps.Include(u => u.Medicine).Include(u => u.Patient).Include(u => u.RemindTime);
            return View(usermedicinemaps.ToList());
        }

        //
        // GET: /UserMedicineMap/Details/5

        public ActionResult Details(int id = 0)
        {
            UserMedicineMap usermedicinemap = db.UserMedicineMaps.Find(id);
            if (usermedicinemap == null)
            {
                return HttpNotFound();
            }
            return View(usermedicinemap);
        }

        //
        // GET: /UserMedicineMap/Create

        public ActionResult Create()
        {
            ViewBag.MedicineId = new SelectList(db.Medicines, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Patients, "Id", "FirstName");
            ViewBag.RemindTimeId = new SelectList(db.RemindTimes, "Id", "RemindValue");
            return View();
        }

        //
        // POST: /UserMedicineMap/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserMedicineMap usermedicinemap)
        {
            if (ModelState.IsValid)
            {
                db.UserMedicineMaps.Add(usermedicinemap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MedicineId = new SelectList(db.Medicines, "Id", "Name", usermedicinemap.MedicineId);
            ViewBag.UserId = new SelectList(db.Patients, "Id", "FirstName", usermedicinemap.UserId);
            ViewBag.RemindTimeId = new SelectList(db.RemindTimes, "Id", "RemindValue", usermedicinemap.RemindTimeId);
            return View(usermedicinemap);
        }

        //
        // GET: /UserMedicineMap/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserMedicineMap usermedicinemap = db.UserMedicineMaps.Find(id);
            if (usermedicinemap == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicineId = new SelectList(db.Medicines, "Id", "Name", usermedicinemap.MedicineId);
            ViewBag.UserId = new SelectList(db.Patients, "Id", "FirstName", usermedicinemap.UserId);
            ViewBag.RemindTimeId = new SelectList(db.RemindTimes, "Id", "RemindValue", usermedicinemap.RemindTimeId);
            return View(usermedicinemap);
        }

        //
        // POST: /UserMedicineMap/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserMedicineMap usermedicinemap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usermedicinemap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicineId = new SelectList(db.Medicines, "Id", "Name", usermedicinemap.MedicineId);
            ViewBag.UserId = new SelectList(db.Patients, "Id", "FirstName", usermedicinemap.UserId);
            ViewBag.RemindTimeId = new SelectList(db.RemindTimes, "Id", "RemindValue", usermedicinemap.RemindTimeId);
            return View(usermedicinemap);
        }

        //
        // GET: /UserMedicineMap/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserMedicineMap usermedicinemap = db.UserMedicineMaps.Find(id);
            if (usermedicinemap == null)
            {
                return HttpNotFound();
            }
            return View(usermedicinemap);
        }

        //
        // POST: /UserMedicineMap/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserMedicineMap usermedicinemap = db.UserMedicineMaps.Find(id);
            db.UserMedicineMaps.Remove(usermedicinemap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}