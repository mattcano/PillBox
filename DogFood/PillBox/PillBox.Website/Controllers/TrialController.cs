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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private PillBoxDbContext db = new PillBoxDbContext();

        //
        // GET: /Trial/

        public ActionResult Index()
        {
            var patients = db.Set<PillBoxUser>().ToList();
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

            PillBoxUser patient = db.Set<PillBoxUser>()
                .Where(p => p.PhoneNumber.Contains(request.To.Substring(2)))
                .FirstOrDefault();

            //.Where(p => p.PhoneNumber.Contains(request.From.Substring(2)))

            if (patient != null)
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

        public ActionResult ProcessResponse(VoiceRequest request)
        {
            var response = new TwilioResponse();

            PillBoxUser patient = db.Set<PillBoxUser>()
                .Where(p => p.PhoneNumber.Contains(request.To.Substring(3)))
                .FirstOrDefault();

            //.Where(p => p.PhoneNumber.Contains(request.From.Substring(3)))

            Reminder reminder = db.Set<Reminder>().Where(r => r.CallSID == request.CallSid).FirstOrDefault();

            if (request.Digits == "1")
            {
                response.Say("Thank you for responding. We have you down as taking your medicine!");


                if (patient != null)
                {
                    reminder.IsTaken = true;
                    reminder.ResponseTime = DateTime.Now;
                    reminder.ReminderType = Model.Enum.ReminderType.PHONE;
                    reminder.User = patient;

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
                    reminder.User = patient;

                    db.Entry(reminder).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return new TwiMLResult(response);
        }

        public void test()
        {
            PillBoxUser patient = db.Set<PillBoxUser>().Find(2);

            Reminder reminder = db.Set<Reminder>().Where(p => p.User.Id == patient.Id)
                .OrderByDescending(p => p.RemindTimeSent)
                .FirstOrDefault();

            int here = 0;
        }

        [HttpPost]
        public ActionResult ProcessTextResponse(SmsRequest request)
        {
            log.Info("Begin processing text response");
            var response = new TwilioResponse();

            PillBoxUser patient = db.Set<PillBoxUser>()
                .Where(p => p.PhoneNumber.Contains(request.From.Substring(2)))
                .FirstOrDefault();

            log.Info("Patient name: " + patient.FullName);

            Reminder reminder = db.Set<Reminder>().Where(p => p.User.Id == patient.Id)
                .OrderByDescending(p => p.RemindTimeSent)
                .FirstOrDefault();
            log.Info("Reminder Id: " + reminder.Id);

            log.Info("User responded: " + request.Body);
            if (!IsExpired(reminder.RemindTimeSent.Value))
            {
                log.Info("Begin sending twilio response to user");

                if (request.Body.ToLower().Contains('y'))
                {
                    log.Info("User responded yes");
                    response.Sms("Great job! Keep it up. :)");

                    if (patient != null)
                    {
                        log.Info("Begin update response to true");
                        reminder.IsTaken = true;
                        reminder.ResponseTime = DateTime.Now;
                        //reminder.ResponseTime = DateTime.Now.ToUniversalTime();
                        reminder.ReminderType = Model.Enum.ReminderType.SMS;
                        reminder.User = patient;

                        db.Entry(reminder).State = EntityState.Modified;
                        db.SaveChanges();
                        log.Info("End update response to true");
                    }
                }
                else if (request.Body.ToLower().Contains('n'))
                {
                    log.Info("User responded no");
                    response.Sms("That’s not good -- let’s get you back on track tomorrow. We want you to stay healthy for your family! Reply with a comment for your records. Msg rates apply.");

                    if (patient != null)
                    {
                        log.Info("Begin update response to false");
                        reminder.IsTaken = false;
                        reminder.ResponseTime = DateTime.Now;
                        //reminder.ResponseTime = DateTime.Now.ToUniversalTime();
                        reminder.ReminderType = Model.Enum.ReminderType.SMS;
                        reminder.User = patient;

                        db.Entry(reminder).State = EntityState.Modified;
                        db.SaveChanges();
                        log.Info("End update response to false");
                    }
                }
                else
                {
                    log.Info("User responded with an unexpected response: " + request.Body);
                    response.Sms("Sorry we where unable to process that response. Please try again..");
                }
            }
            else
            {
                response.Sms("Sorry your response came too late! Please wait till the next reminder.");
            }

            log.Info("Return twilio response to user text response");
            return new TwiMLResult(response);
        }

        public ActionResult Deactivate(int id)
        {

            PillBoxUser patient = db.Set<PillBoxUser>().Find(id);

            patient.IsInTrial = false;
            patient.AutoSendPhone = false;
            patient.AutoSendSMS = false;
            db.Entry(patient).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult SendSMS(int id)
        {

            PillBoxUser patient = db.Set<PillBoxUser>().Find(id);
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
                newReminder.User = patient;

                db.Set<Reminder>().Add(newReminder);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SendCall(int id)
        {
            PillBoxUser patient = db.Set<PillBoxUser>().Find(id);
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
                newReminder.User = patient;

                db.Set<Reminder>().Add(newReminder);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private string GetMedicinesList(object id)
        {
            string medicines = "";

            PillBoxUser patient = db.Set<PillBoxUser>().Find((Guid)id);

            var medList = patient.Medicines.Select(m => m.Name);

            foreach (var med in medList)
            {
                medicines += med + ", ";
            }

            return medicines;
        }

        private string GetMedicinesListForSms(PillBoxUser patient)
        {
            string medicines = "";
            string truncate = "";

            var medList = patient.Medicines.Select(m => m.Name);

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

        private bool IsExpired(DateTime sentTime)
        {
            bool expired = false;

            TimeSpan elasped = DateTime.Now.Subtract(sentTime);

            if (elasped.TotalMinutes > 175)
            {
                log.Info("Is expired.");
                expired = true;
            }

            log.Info("Not expired");
            return expired;
        }
    }
}
