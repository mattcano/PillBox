using PillBox.DAL.Entities;
using PillBox.Model.Entities;
using PillBox.Services;
using PillBox.Services.Interfaces;
using PillBox.Website.Controllers;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace PillBox.Website.ScheduledTasks
{
    public class PillBoxEmailerJob : IJob
    {
        PillBoxContext db;

        public void Execute(IJobExecutionContext context)
        {
            db = new PillBoxContext();

            var users = db.Patients.ToList();

            foreach (var user in users)
            {

                if (user.FirstName == "Damola")
                {
                    List<Reminder> userReminders = new List<Reminder>();

                    // Initialize StringWriter instance.
                    StringWriter stringWriter = new StringWriter();

                    // Put HtmlTextWriter in using block because it needs to call Dispose.
                    HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

                    //writer.AddAttribute

                    foreach (var medicine in user.Medicines)
                    {
                        userReminders.Add(new Reminder()
                        {
                            IsTaken = false,
                        });

                        writer.RenderBeginTag(HtmlTextWriterTag.Div); //Begin #1
                        writer.RenderBeginTag(HtmlTextWriterTag.Span); //Begin #2
                        writer.Write(medicine.Name);
                        writer.RenderEndTag(); // End #2
                        writer.RenderEndTag(); // End #1
                    }

                    EmailerService emailService = new EmailerService();
                    emailService.SendEmailTo(user.Email, stringWriter.ToString());
                }
            }
        }
    }

    public class ReminderJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            string remindTime = dataMap.GetString("RemindTime");

            // Get all currently active trial patients
            // who have with auto send sms or phone set to true
            PillBoxContext dbContext = new PillBoxContext();

            var trailPatientsCall = dbContext.Patients
                .Where(p => p.IsInTrial == true)
                .Where(p => p.AutoSendPhone == true).ToList()
                .ToList();

            ITwilioService twilioService = new TwilioService();

            foreach (var patient in trailPatientsCall)
            {
                twilioService.SendPhoneCall(patient);
            }

            Thread.Sleep(1000);

            var trailPatientsSms = dbContext.Patients
                .Where(p => p.IsInTrial == true)
                .Where(p => p.AutoSendSMS == true).ToList()
                .ToList();

            foreach (var patient in trailPatientsSms)
            {
                twilioService.SendSMS(patient);
            }
        }
    }
}