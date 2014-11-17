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
    public class ReminderController : Controller
    {
        private PillBoxDBContext db = new PillBoxDBContext();

        //
        // GET: /Reminder/

        public ActionResult Index()
        {
            var reminders = db.Reminders.Include(r => r.RemindTime).Include(r => r.UserMedicineMap);
            return View(reminders.ToList());
        }

        //
        // GET: /Reminder/Details/5

        public ActionResult Details(int id = 0)
        {
            Reminder reminder = db.Reminders.Find(id);
            if (reminder == null)
            {
                return HttpNotFound();
            }
            return View(reminder);
        }

        //
        // GET: /Reminder/Create

        public ActionResult Create()
        {
            ViewBag.RemindTimeId = new SelectList(db.RemindTimes, "Id", "RemindValue");
            ViewBag.UserMedicineMapId = new SelectList(db.UserMedicineMaps, "Id", "Id");
            return View();
        }

        //
        // POST: /Reminder/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                db.Reminders.Add(reminder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RemindTimeId = new SelectList(db.RemindTimes, "Id", "RemindValue", reminder.RemindTimeId);
            ViewBag.UserMedicineMapId = new SelectList(db.UserMedicineMaps, "Id", "Id", reminder.UserMedicineMapId);
            return View(reminder);
        }

        //
        // GET: /Reminder/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Reminder reminder = db.Reminders.Find(id);
            if (reminder == null)
            {
                return HttpNotFound();
            }
            ViewBag.RemindTimeId = new SelectList(db.RemindTimes, "Id", "RemindValue", reminder.RemindTimeId);
            ViewBag.UserMedicineMapId = new SelectList(db.UserMedicineMaps, "Id", "Id", reminder.UserMedicineMapId);
            return View(reminder);
        }

        //
        // POST: /Reminder/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reminder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RemindTimeId = new SelectList(db.RemindTimes, "Id", "RemindValue", reminder.RemindTimeId);
            ViewBag.UserMedicineMapId = new SelectList(db.UserMedicineMaps, "Id", "Id", reminder.UserMedicineMapId);
            return View(reminder);
        }


        //
        // POST: /Reminder
        public ActionResult EmailResult()
        {
            return View();
        }


        //
        // GET: /Reminder/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Reminder reminder = db.Reminders.Find(id);
            if (reminder == null)
            {
                return HttpNotFound();
            }
            return View(reminder);
        }

        //
        // POST: /Reminder/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reminder reminder = db.Reminders.Find(id);
            db.Reminders.Remove(reminder);
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