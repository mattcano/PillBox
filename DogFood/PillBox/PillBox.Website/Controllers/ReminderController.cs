using PillBox.DAL.Entities;
using PillBox.Model.Entities;
using PillBox.Website.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PillBox.Website.Controllers
{
    public class ReminderController : Controller
    {
        private PillBoxContext db = new PillBoxContext();

        public List<ReminderQuestion> GetReminderQuestionsFromDB()
        {
            List<ReminderQuestion> reminderQuestions = new List<ReminderQuestion>();

            var reminders = db.Reminders.Include(r => r.RemindTime).Include(r => r.UserMedicineMap);

            foreach (var reminder in reminders)
            {
                reminderQuestions.Add(new ReminderQuestion() { Id = reminder.Id, Reminder = reminder });
            }

            return reminderQuestions;
        }


        public ActionResult Eval()
        {
            var evalVM = new Evaluation();
            evalVM.Questions = GetReminderQuestionsFromDB();

            return View(evalVM);
        }

        [HttpPost]
        public ActionResult Eval(Evaluation eval)
        {
            foreach (var q in eval.Questions)
            {
                var rId = q.Id;
                var selectedAnswer = q.SurveyResponse;
                Reminder localReminder = db.Reminders.Find(rId);
                localReminder.ResponseTime = DateTime.Now;

                if (selectedAnswer == 1)
                {
                    localReminder.IsTaken = true;
                }
                else if (selectedAnswer == 2)
                {
                    localReminder.IsTaken = false;
                }
                else
                { 
                    // Generate a new reminder
                }
                db.Reminders.Attach(localReminder);
                var entry = db.Entry(localReminder);
                entry.Property(e => e.IsTaken).IsModified = true;
                entry.Property(e => e.ResponseTime).IsModified = true;
                db.SaveChanges();
            }
            
            return View();
        }

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