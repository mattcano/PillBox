using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using Quartz.Impl;
using System.Diagnostics;
using PillBox.Services;
using System.IO;
using System.Web.UI;
using PillBox.DAL.Entities;
using System.Linq;
using System.Collections.Generic;
using PillBox.Model.Entities;
using System.Text;
using Twilio;

namespace PillBox.IntegrationTest
{
    [TestClass]
    public class Experiments
    {

        [TestMethod]
        public void Google_Post_Test()
        {

            // Initialize StringWriter instance.
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
            StringBuilder builder = new StringBuilder();

            builder.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            builder.Append("<head>");
            builder.Append("	<title>Programmer Questionnaire</title>");
            builder.Append("</head>");
            builder.Append("<body>");
            builder.Append("	<form method=\"post\" action=\"http://ec2-54-67-55-4.us-west-1.compute.amazonaws.com/Home/DatabaseMe\">");
            builder.Append("		<div>");
            builder.Append("			Enter your first name:&nbsp;");
            builder.Append("			<input type=\"text\" name=\"FirstName\" />");
            builder.Append("			<br />");
            builder.Append("			Enter your last name:&nbsp;");
            builder.Append("			<input type=\"text\" name=\"LastName\" />");
            builder.Append("			<br /><br />");
            builder.Append("			You program with:");
            builder.Append("			<br />&nbsp;&nbsp;&nbsp;");
            builder.Append("			<input type=\"checkbox\" name=\"CS\" />C#");
            builder.Append("			<br />&nbsp;&nbsp;&nbsp;");
            builder.Append("			<input type=\"checkobox\" name=\"VB\" />VB .NET");
            builder.Append("			<br /><br />");
            builder.Append("			<input type=\"submit\" value=\"Submit\" id=\"OK\" />");
            builder.Append("		</div>");
            builder.Append("	</form>");
            builder.Append("</body>");
            builder.Append("</html>");

            writer.Write(builder.ToString());

            EmailerService emailService = new EmailerService();
            emailService.SendEmailTo("damola.omotosho@gmail.com", builder.ToString());

        }

        [TestMethod]
        public void Quartz_Lesson_1_Sample_Code()
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("myTrigger", "group1")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(40)
                  .RepeatForever())
              .Build();

            sched.ScheduleJob(job, trigger);

            Console.ReadLine();
        }

        [TestMethod]
        public void Quartz_Emailer_Sample()
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<EmailerJob>()
                .WithIdentity("myJob", "group1")
                .Build();


            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("myTrigger", "group1")
              .StartNow()
             .WithSimpleSchedule(x => x
                  .WithRepeatCount(1)
                  .WithIntervalInSeconds(10))
              .Build();
            
            
            //// Trigger the job to run now, and then every 40 seconds
            //ITrigger trigger = TriggerBuilder.Create()
            //  .WithIdentity("myTrigger", "group1")
            //  .StartNow()
            //  .WithSimpleSchedule(x => x
            //      .WithIntervalInSeconds(20)
            //      .RepeatForever())
            //  .Build();

            sched.ScheduleJob(job, trigger);

            Console.ReadLine();
        }

        [TestMethod]
        public void HTML_TextWriter_Sample()
        {
            Debug.WriteLine(GetDivElements());

            Console.ReadLine();

        }


        [TestMethod]
        public void Quartz_Emailer_From_DataSample()
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<PillBoxEmailerJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("myTrigger", "group1")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithRepeatCount(0))
              .Build();

            sched.ScheduleJob(job, trigger);

            Console.ReadLine();
        }

        public class PillBoxEmailerJob : IJob
        {
            PillBoxDbContext db;

            public void Execute(IJobExecutionContext context)
            {
                db = new PillBoxDbContext();

                var users = db.Set<PillBoxUser>();

                foreach (var user in users)
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
                            Medicine = medicine,
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

        public class HelloJob : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                Debug.WriteLine("Greetings from HelloJob!");
            }
        }

        public class EmailerJob : IJob
        {
            static EmailerService emailService = new EmailerService();

            public void Execute(IJobExecutionContext context)
            {
                emailService.SendEmailTo("damola.omotosho@gmail.com", Experiments.GetDivElements());
            }
        }

        public class TwilioSmsJob : IJob
        {
            static TwilioService twilioService = new TwilioService();

            public void Execute(IJobExecutionContext context)
            {
                JobKey key = context.JobDetail.Key;

                JobDataMap dataMap = context.Trigger.JobDataMap;

                string userId = dataMap.GetString("userId");
                int medicineId = dataMap.GetInt("medicineId");
                string phoneNumber = dataMap.GetString("phoneNumber");
                string medicine = dataMap.GetString ("medicine");

                string reminderMessage =  
                    "Hello! This is a PillBox reminder to take your " + medicine + ". Reply Y if you’ve done so, N if not. Msg rates apply.";

                twilioService.SendSMS(userId, medicineId, phoneNumber, reminderMessage);
            }
        }

        [TestMethod]
        public void Quartz_Cron_Twilio_Text()
        {
            PillBoxDbContext db = new PillBoxDbContext();

            // Construct a scheduler factory
            ISchedulerFactory schedFactory = new StdSchedulerFactory();

            // Get a scheduler
            IScheduler sched = schedFactory.GetScheduler();
            sched.Start();

            var user = db.Set<PillBoxUser>().FirstOrDefault(u => u.FirstName ==  "Damola");
            var medicines = db.Set<Medicine>().Where(m => m.User.FirstName == "Damola").ToList();

            if((user!= null) && medicines.Count != 0)
            {
                int medicineId = medicines[0].Id;
                string medicine = medicines[0].Name;
                int remindHour = Int32.Parse(medicines[0].RemindTime.Value.ToString("HH"));
                int remindMinute = medicines[0].RemindTime.Value.Minute;
                string phoneNumber = user.PhoneNumber;
                string userId = user.Id;

                IJobDetail job = JobBuilder.Create<TwilioSmsJob>()
                    .WithIdentity(medicineId.ToString(), "JobInfo")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(medicineId.ToString(), "TriggerInfo")
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(remindHour, remindMinute))
                    .UsingJobData("phoneNumber", phoneNumber)
                    .UsingJobData("userId", userId)
                    .UsingJobData("medicine", medicine)
                    .UsingJobData("medicineId", medicineId)
                    .ForJob(job)
                    .Build();

                sched.ScheduleJob(job, trigger);

                Console.ReadLine();
            }
            
        }
        
        [TestMethod]
        public void Can_Send_SMS_From_Twilio()
        {
            string accountSid = "ACa68cb3055a5c573f76862831c0995c48";
            string authToken = "8917e0e37320d868756ca59864dd29b6";
            var client = new TwilioRestClient(accountSid, authToken);

            Message result = client.SendMessage(
                "6506668667",
                "3014373223", 
                "Testing out twilio from .NET"
                );

            if(result.RestException != null)
            {
                Assert.Fail(result.RestException.Message);
            }

        }


        [TestMethod]
        public void Can_Send_Twilio_Voice_Call_And_Recieve_Responses()
        {

        }

        public static string GetDivElements()
        {
            string[] _words = { "Sam", "Dot", "Perls" };

            // Initialize StringWriter instance.
            StringWriter stringWriter = new StringWriter();

            // Put HtmlTextWriter in using block because it needs to call Dispose.
            HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

            // Loop over some strings.
            foreach (var word in _words)
            {
                // Some strings for the attributes.
                string classValue = "ClassName";
                string urlValue = "http://www.dotnetperls.com/";
                string imageValue = "image.jpg";

                // The important part:
                writer.AddAttribute(HtmlTextWriterAttribute.Class, classValue);
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // Begin #1

                writer.AddAttribute(HtmlTextWriterAttribute.Href, urlValue);
                writer.RenderBeginTag(HtmlTextWriterTag.A); // Begin #2

                writer.AddAttribute(HtmlTextWriterAttribute.Src, imageValue);
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "60");
                writer.AddAttribute(HtmlTextWriterAttribute.Height, "60");
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, "");

                writer.RenderBeginTag(HtmlTextWriterTag.Img); // Begin #3
                writer.RenderEndTag(); // End #3

                writer.Write(word);

                writer.RenderEndTag(); // End #2
                writer.RenderEndTag(); // End #1
            }

            // Return the result.
            return stringWriter.ToString();
        }
    }
}
