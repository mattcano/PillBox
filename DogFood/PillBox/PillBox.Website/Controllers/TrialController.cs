using PillBox.DAL.Entities;
using PillBox.Model.Entities;
using PillBox.Website.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace PillBox.Website.Controllers
{
    public class TrialController : Controller
    {
        private PillBoxContext db = new PillBoxContext();

        //
        // GET: /Trial/

        public ActionResult Index()
        {
            var patients = db.Patients.ToList();
            List<TrialPatientViewModel> trailPatients = 
                new List<TrialPatientViewModel>(patients.Select(tp => new TrialPatientViewModel(tp)));

            TrialManagerViewModel tmViewModel = new TrialManagerViewModel();
            tmViewModel.TrialPatients = trailPatients;

            return View(tmViewModel);
        }

        public TwiMLResult GetResponse(VoiceRequest request)
        {
            var response = new TwilioResponse();

            string name;

            Patient patient = db.Set<Patient>()
                .Where(p => p.PhoneNumber.Contains(request.To.Substring(2)))
                .FirstOrDefault();

            //.Where(p => p.PhoneNumber.Contains(request.From.Substring(2)))

            if(patient != null)
            {
                name = patient.FirstName;
            }
            else
            { 
                name = "You";
            }

            response.BeginGather(new { action = "http://71.237.221.15/pillbox/trial/ProcessResponse", numDigits = "1" })
                    .Say("Hey " + name + " ! This is a pillbox reminder. Have you taken your medicines?"
                            + "Press 1 for yes. Press 2 for no")
                    .EndGather();

            return new TwiMLResult(response);
        }

        public ActionResult ProcessResponse(VoiceRequest request)
        {
            var response = new TwilioResponse();

            Patient patient = db.Set<Patient>()
                .Where(p => p.PhoneNumber.Contains(request.To.Substring(3)))
                .FirstOrDefault();

            //.Where(p => p.PhoneNumber.Contains(request.From.Substring(3)))

            Reminder reminder = db.Set<Reminder>().Where(r => r.CallSID == request.CallSid).FirstOrDefault();

            if (request.Digits == "1")
            {
                response.Say("Thank you for responding. We have you down as taking your medicine!");


                if(patient != null)
                {
                    reminder.IsTaken = true;
                    reminder.ResponseTime = DateTime.Now;
                    reminder.ReminderType = Model.Enum.ReminderType.PHONE;
                    reminder.Patient = patient;

                    db.Entry(reminder).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                response.Say("Thank you for responding. We have you down as NOT taking your medicine!");
                
                if (patient != null)
                {
                    reminder.IsTaken = false;
                    reminder.ResponseTime = DateTime.Now;
                    reminder.ReminderType = Model.Enum.ReminderType.PHONE;
                    reminder.Patient = patient;

                    db.Entry(reminder).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return new TwiMLResult(response);
        }

        public ActionResult SendSMS(int id)
        {

            Patient patient = db.Set<Patient>().Find(id);
            string accountSid = "ACa68cb3055a5c573f76862831c0995c48";
            string authToken = "8917e0e37320d868756ca59864dd29b6";
            var client = new TwilioRestClient(accountSid, authToken);

            Twilio.Message result = client.SendMessage(
                "4248357603",
                patient.PhoneNumber, 
                "Testing out twilio from .NET");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SendCall(int id, string remindMessage)
        {

            var temp = remindMessage;
            
            Patient patient = db.Set<Patient>().Find(id);
            string accountSid = "ACa68cb3055a5c573f76862831c0995c48";
            string authToken = "8917e0e37320d868756ca59864dd29b6";
            var client = new TwilioRestClient(accountSid, authToken);


            var call = client.InitiateOutboundCall("4248357603",
                patient.PhoneNumber,
                 "http://71.237.221.15/pillbox/Trial/GetResponse");
            
            if (patient != null)
            {
                Reminder newReminder = new Reminder();

                newReminder.IsTaken = false;
                newReminder.RemindSendTime = DateTime.Now;
                newReminder.CallSID = call.Sid;
                newReminder.ReminderType = Model.Enum.ReminderType.PHONE;
                newReminder.Patient = patient;

                db.Set<Reminder>().Add(newReminder);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        ////
        //// GET: /Trial/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    TrialPatientViewModel trialpatientviewmodel = db.TrialPatientViewModels.Find(id);
        //    if (trialpatientviewmodel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(trialpatientviewmodel);
        //}

        ////
        //// GET: /Trial/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Trial/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(TrialPatientViewModel trialpatientviewmodel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.TrialPatientViewModels.Add(trialpatientviewmodel);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(trialpatientviewmodel);
        //}

        ////
        //// GET: /Trial/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    TrialPatientViewModel trialpatientviewmodel = db.TrialPatientViewModels.Find(id);
        //    if (trialpatientviewmodel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(trialpatientviewmodel);
        //}

        ////
        //// POST: /Trial/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(TrialPatientViewModel trialpatientviewmodel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(trialpatientviewmodel).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(trialpatientviewmodel);
        //}

        ////
        //// GET: /Trial/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    TrialPatientViewModel trialpatientviewmodel = db.TrialPatientViewModels.Find(id);
        //    if (trialpatientviewmodel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(trialpatientviewmodel);
        //}

        ////
        //// POST: /Trial/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    TrialPatientViewModel trialpatientviewmodel = db.TrialPatientViewModels.Find(id);
        //    db.TrialPatientViewModels.Remove(trialpatientviewmodel);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
