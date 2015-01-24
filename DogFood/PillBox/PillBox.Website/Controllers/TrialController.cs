using PillBox.Core;
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

            PillboxUser patient = db.Set<PillboxUser>()
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

            response.BeginGather(new { action = "http://ec2-54-67-55-4.us-west-1.compute.amazonaws.com/trial/ProcessResponse", numDigits = "1" })
                    .Say("Hey " + name + " ! This is a pillbox reminder. Have you taken your " + GetMedicinesList(patient.Id)
                            + "? Press 1 for yes. Press 2 for no")
                    .EndGather();

            return new TwiMLResult(response);
        }

        private string GetMedicinesList(int id)
        {
            string medicines = "";

            PillboxUser patient = db.Set<PillboxUser>().Find(id);

            var medList = patient.Medicines.Select(m => m.Medicine.Name);

            foreach (var med in medList)
            {
                medicines += med + ", ";
            }

            return medicines;
        }

        private string GetMedicinesListForSms(PillboxUser patient)
        {
            string medicines = "";
            string truncate = "";

            var medList = patient.Medicines.Select(m => m.Medicine.Name);

            foreach (var med in medList)
            {
                medicines += med + ", ";
            }

            if (medList.Count() > 0)
            {
                truncate = medicines.Remove(medicines.Length - 2, 2);
            }

            return truncate;
        }

        public ActionResult ProcessResponse(VoiceRequest request)
        {
            var response = new TwilioResponse();

            PillboxUser patient = db.Set<PillboxUser>()
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

        public void test()
        {
            PillboxUser patient = db.Set<PillboxUser>().Find(2);

            Reminder reminder = db.Set<Reminder>().Where(p => p.Patient.Id == patient.Id)
                .OrderByDescending(p => p.RemindTimeSent)
                .FirstOrDefault();

            int here = 0;
        }

        [HttpPost]
        public ActionResult ProcessTextResponse(SmsRequest request)
        {
            var response = new TwilioResponse();

            PillboxUser patient = db.Set<PillboxUser>()
                .Where(p => p.PhoneNumber.Contains(request.From.Substring(2)))
                .FirstOrDefault();

            Reminder reminder = db.Set<Reminder>().Where(p => p.Patient.Id == patient.Id)
                .OrderByDescending(p => p.RemindTimeSent)
                .FirstOrDefault();

            if (!IsExpired(reminder.RemindTimeSent.Value))
            {
                if (request.Body.ToLower().Contains('y'))
                {
                    response.Sms("Great job! Keep it up. :)");

                    if (patient != null)
                    {
                        reminder.IsTaken = true;
                        reminder.ResponseTime = DateTime.Now;
                        reminder.ReminderType = Model.Enum.ReminderType.SMS;
                        reminder.Patient = patient;

                        db.Entry(reminder).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else if (request.Body.ToLower().Contains('n'))
                {
                    response.Sms("That’s not good -- let’s get you back on track tomorrow. We want you to stay healthy for your family! Reply with a comment for your records. Msg rates apply.");

                    if (patient != null)
                    {
                        reminder.IsTaken = false;
                        reminder.ResponseTime = DateTime.Now;
                        reminder.ReminderType = Model.Enum.ReminderType.SMS;
                        reminder.Patient = patient;

                        db.Entry(reminder).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    response.Sms("Sorry we where unable to process that response. Please try again..");
                }
            }
            else
            {
                response.Sms("Sorry your response came too late! Please wait till the next reminder.");
            }

            return new TwiMLResult(response);
        }

        public ActionResult Deactivate(int id)
        {

            PillboxUser patient = db.Set<PillboxUser>().Find(id);

            patient.IsInTrial = false;
            patient.AutoSendPhone = false;
            patient.AutoSendSMS = false;
            db.Entry(patient).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool IsExpired(DateTime sentTime)
        {
            bool expired = false;

            TimeSpan elasped = DateTime.Now.Subtract(sentTime);

            if (elasped.TotalMinutes > 175)
            {
                expired = true;
            }

            return expired;
        }

        public ActionResult SendSMS(int id)
        {

            PillboxUser patient = db.Set<PillboxUser>().Find(id);
            string accountSid = "ACa68cb3055a5c573f76862831c0995c48";
            string authToken = "8917e0e37320d868756ca59864dd29b6";
            var client = new TwilioRestClient(accountSid, authToken);

            var sms = client.SendSmsMessage(Constants.TWILIO_NUMBER,
             patient.PhoneNumber,

             "Hello! This is your reminder to take your "
             +
             GetMedicinesListForSms(patient)
             +
             ". Reply Y if you’ve done so, N if not. Msg rates apply.");

            if (patient != null)
            {
                Reminder newReminder = new Reminder();

                newReminder.IsTaken = false;
                newReminder.RemindTimeSent = DateTime.Now;
                newReminder.MessageSID = sms.Sid;
                newReminder.ReminderType = Model.Enum.ReminderType.SMS;
                newReminder.Patient = patient;

                db.Set<Reminder>().Add(newReminder);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SendCall(int id)
        {            
            PillboxUser patient = db.Set<PillboxUser>().Find(id);
            string accountSid = "ACa68cb3055a5c573f76862831c0995c48";
            string authToken = "8917e0e37320d868756ca59864dd29b6";
            var client = new TwilioRestClient(accountSid, authToken);


            var call = client.InitiateOutboundCall("4248357603",
                patient.PhoneNumber,
                 "http://ec2-54-67-55-4.us-west-1.compute.amazonaws.com/Trial/GetResponse");
            
            if (patient != null)
            {
                Reminder newReminder = new Reminder();

                newReminder.IsTaken = false;
                newReminder.RemindTimeSent = DateTime.Now;
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
