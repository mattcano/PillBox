using PillBox.DAL.Entities;
using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PillBox.Website.Controllers
{
    public class PatientController : Controller
    {
        private PillBoxContext db = new PillBoxContext();

        //
        // GET: /Patient/

        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }

        //
        // GET: /Patient/Details/5

        public ActionResult Details(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        //
        // GET: /Patient/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Patient/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        //
        // GET: /Patient/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        //
        // POST: /Patient/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        //
        // GET: /Patient/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        //
        // POST: /Patient/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult WeeklySummary(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
