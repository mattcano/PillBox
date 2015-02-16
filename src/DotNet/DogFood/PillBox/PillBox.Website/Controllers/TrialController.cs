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

        //public ActionResult DataFeed()
        //{
        //    var db
        //}

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

            try
            {
                PillBoxUser patient = db.Set<PillBoxUser>()
                    .Where(p => p.PhoneNumber.Contains(request.From.Substring(2)))
                    .FirstOrDefault();

                // TODO None of this other logic makes sense if there
                // is no matching patient send user message
                // telling them to contact you at damola.omotosho@gmail.com

                log.Info("Patient name: " + patient.FullName);

                Reminder reminder = db.Set<Reminder>().Where(p => p.User.Id == patient.Id && p.ResponseTime == null)
                    .OrderByDescending(p => p.RemindTimeSent)
                    .FirstOrDefault();

                // TODO if the patient is valid
                // but there is no unanswered reminder for them
                // and you know that they where supposed to recieve/generate
                // reminders for medicines today that have not been generated
                // do the reminder generation for the day. Add chaining if there
                // were multiple medicines they were supposed to take.

                log.Info("Reminder Id: " + reminder.Id);

                log.Info("User responded: " + request.Body);

                if (reminder == null)
                {
                    log.Info("Unable to find a valid reminder for this user response");
                    log.Info("Text recieved from: " + request.From + " Message: " + request.Body);
                    response.Sms("Thank you. We already recorded your response earlier.");
                }
                else if (!IsExpired(reminder))
                {
                    log.Info("Begin sending twilio response to user");

                    if ((request.Body.ToLower().Contains('y') ||
                        request.Body.ToLower().Contains("yes")) && request.Body.Length <= 4)
                    {
                        log.Info("User responded yes");
                        response.Sms("Great job! Keep it up. :)");

                        if (patient != null)
                        {
                            log.Info("Begin update response to true");
                            reminder.IsTaken = true;
                            reminder.ResponseTime = DateTime.Now;
                            reminder.ReminderType = Model.Enum.ReminderType.SMS;
                            reminder.Message = request.Body;
                            reminder.User = patient;

                            db.Entry(reminder).State = EntityState.Modified;
                            db.SaveChanges();
                            log.Info("End update response to true");
                        }
                    }
                    else
                    {
                        log.Info("User response non-standard");

                        if (patient != null)
                        {
                            log.Info("Begin update non standard response");
                            reminder.ResponseTime = DateTime.Now;
                            reminder.ReminderType = Model.Enum.ReminderType.SMS;
                            reminder.Message = request.Body;
                            reminder.User = patient;

                            db.Entry(reminder).State = EntityState.Modified;
                            db.SaveChanges();
                            log.Info("End update non standard response");
                        }
                        log.Info("User responded with: " + request.Body);
                        response.Sms("Thank you. Your response has been recorded.");
                    }
                }
                //else if (IsExpired(reminder))
                //{
                //    log.Info("User responded too late");

                //    if (patient != null)
                //    {
                //        log.Info("Begin update late response");
                //        reminder.IsTaken = false;
                //        reminder.ResponseTime = DateTime.Now;
                //        reminder.ReminderType = Model.Enum.ReminderType.SMS;
                //        reminder.Message = "@LATE -" + request.Body;
                //        reminder.User = patient;

                //        db.Entry(reminder).State = EntityState.Modified;
                //        db.SaveChanges();
                //        log.Info("End update late response");
                //    }

                //    log.Info("User responded with: " + request.Body);
                //}
                else
                {
                    log.Info("Reminder generation error.");
                    log.Info("User responded with: " + request.Body);
                    response.Sms("Thank you. Your response has been recorded in our logs. Thanks for being an awesome beta tester!");
                }
            }
            catch(Exception ex)
            {
                ///TODO this is just bad too much logic
                ///in a controller
                log.Info("An exception occurred",ex);
                log.Info("Text recieved from: " + request.From + " Message: " + request.Body);

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


            var call = client.InitiateOutboundCall("6506668667",
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

        private bool IsExpired(Reminder reminder)
        {
            bool expired = false;

            TimeSpan elasped = DateTime.Now.Subtract(reminder.RemindTimeSent.Value);

            if (elasped.TotalHours >= 24)
            {
                log.Info("Is expired.");
                expired = true;
                return expired;
            }

            log.Info("Not expired");
            return expired;
        }
    }
}
