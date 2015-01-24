using PillBox.Core;
using PillBox.DAL.Entities;
using PillBox.Model.Entities;
using PillBox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twilio;

namespace PillBox.Services
{
    public class TwilioService:ITwilioService, IDisposable
    {
        PillBoxContext context;
        TwilioRestClient client;

        public TwilioService()
        {
            context = new PillBoxContext();
            client = new TwilioRestClient
                (Constants.TWILIO_ACCOUNTSID, 
                    Constants.TWILIO_AUTHTOKEN);
        }

        public void SendSMS(PillboxUser patient)
        {

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

                context.Set<Reminder>().Add(newReminder);
                context.SaveChanges();
            }
            
        }

        private string GetMedicinesListForSms(PillboxUser patient)
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

        public void SendSMS(PillboxUser patient, string message)
        {
            throw new NotImplementedException();
        }

        public void UpdateResponseDB(int responseId)
        {
            throw new NotImplementedException();
        }

        public void SendPhoneCall(PillboxUser patient)
        {
            var call = client.InitiateOutboundCall(
                Constants.TWILIO_NUMBER,
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

                context.Set<Reminder>().Add(newReminder);
                context.SaveChanges();
            }
        }

        public void SendPhoneCall(PillboxUser patient, string message)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
