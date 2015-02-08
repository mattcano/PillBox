using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class TrialController : Controller
    {
        private PillBoxDBContext db = new PillBoxDBContext();

        //
        // GET: /Trial/

        public ActionResult Index()
        {
            return View(db.TrialPatientViewModels.ToList());
        }

        //
        // GET: /Trial/Details/5

        public ActionResult Details(int id = 0)
        {
            TrialPatientViewModel trialpatientviewmodel = db.TrialPatientViewModels.Find(id);
            if (trialpatientviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(trialpatientviewmodel);
        }

        //
        // GET: /Trial/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Trial/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrialPatientViewModel trialpatientviewmodel)
        {
            if (ModelState.IsValid)
            {
                db.TrialPatientViewModels.Add(trialpatientviewmodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trialpatientviewmodel);
        }

        //
        // GET: /Trial/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TrialPatientViewModel trialpatientviewmodel = db.TrialPatientViewModels.Find(id);
            if (trialpatientviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(trialpatientviewmodel);
        }

        //
        // POST: /Trial/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TrialPatientViewModel trialpatientviewmodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trialpatientviewmodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trialpatientviewmodel);
        }

        //
        // GET: /Trial/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TrialPatientViewModel trialpatientviewmodel = db.TrialPatientViewModels.Find(id);
            if (trialpatientviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(trialpatientviewmodel);
        }

        //
        // POST: /Trial/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrialPatientViewModel trialpatientviewmodel = db.TrialPatientViewModels.Find(id);
            db.TrialPatientViewModels.Remove(trialpatientviewmodel);
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