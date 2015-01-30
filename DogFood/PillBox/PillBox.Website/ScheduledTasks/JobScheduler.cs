﻿using PillBox.DAL.Entities;
using PillBox.Model.Entities;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.ScheduledTasks
{
    public class JobScheduler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static PillBoxDbContext db = new PillBoxDbContext();
        static IScheduler scheduler;

        static JobScheduler()
        {
            if (scheduler == null)
            {
                scheduler = StdSchedulerFactory.GetDefaultScheduler();
            }
        }

        public static void Start()
        {
            log.Info("Starting job scheduler");
            scheduler.Start();
            ScheduleServerPingEvery2Mins();
            log.Info("End job scheduler");
        }

        public static void AddJob(IJobDetail job, ITrigger trigger)
        {
            scheduler.ScheduleJob(job, trigger);
        }

        private static void ScheduleServerPingEvery2Mins()
        {
            log.Info("Begin 2 minute server pinging");
            // define the job and tie it to our PingJob class
            IJobDetail job = JobBuilder.Create<PingJob>()
                .WithIdentity("ServerPing", "ServerPing")
                .Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("ServerPing", "ServerPing")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(120)
                    .RepeatForever())
                .Build();

            AddJob(job, trigger);
        }

        public static void ScheduleCurrentMedicineReminders()
        {
            log.Info("Reschedule existing medicine reminders");
            var medicines = db.Set<Medicine>().Include("User");

            foreach (var med in medicines)
            {
                ScheduleMedicineReminder(med);
            }
        }

        public static void EnqueueOnRemindTimes()
        {
            // Job Details
            IJobDetail earlyMorningDetail =
                JobBuilder.Create<ReminderJob>()
                    .UsingJobData("RemindTime", "Early Morning")
                    .Build();

            IJobDetail morningDetail =
                JobBuilder.Create<ReminderJob>()
                    .UsingJobData("RemindTime", "Morning")
                    .Build();

            IJobDetail earlyAfternoonDetail =
                JobBuilder.Create<ReminderJob>()
                    .UsingJobData("RemindTime", "Early Afternoon")
                    .Build();

            IJobDetail afternoonDetail =
                JobBuilder.Create<ReminderJob>()
                    .UsingJobData("RemindTime", "Afternoon")
                    .Build();

            IJobDetail eveningDetail =
                JobBuilder.Create<ReminderJob>()
                    .UsingJobData("RemindTime", "Evening")
                    .Build();

            IJobDetail nightDetail =
                JobBuilder.Create<ReminderJob>()
                    .UsingJobData("RemindTime", "Night")
                    .Build();

            //// Test

            IJobDetail testDetail = JobBuilder.Create<PillBoxEmailerJob>()
                .Build();
            ITrigger testTrigger = TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(4, 37))
                .Build();

            AddJob(testDetail, testTrigger);


            // Triggers
            ITrigger earlyMorningTrigger =
                TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(6, 0))
                .Build();

            ITrigger morningTrigger =
                TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(9, 0))
                .Build();

            ITrigger earlyAfternoonTrigger =
                TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(12, 0))
                .Build();

            ITrigger afternoonTrigger =
                TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(15, 0))
                .Build();

            ITrigger eveningTrigger =
                TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(18, 0))
                .Build();

            ITrigger nightTrigger =
                TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(21, 0))
                .Build();

            AddJob(earlyMorningDetail, earlyMorningTrigger);
            AddJob(morningDetail, morningTrigger);
            AddJob(earlyAfternoonDetail, earlyAfternoonTrigger);
            AddJob(afternoonDetail, afternoonTrigger);
            AddJob(eveningDetail, eveningTrigger);
            AddJob(nightDetail, nightTrigger);
        }

        public static void ScheduleMedicineReminder(Medicine med)
        {
            if ((!string.IsNullOrEmpty(med.UserId)))
            {
                int medicineId = med.Id;
                string medicine = med.Name;
                int remindHour = Int32.Parse(med.RemindTime.Value.ToString("HH"));
                int remindMinute = med.RemindTime.Value.Minute;
                string phoneNumber = med.User.PhoneNumber;
                string userId = med.User.Id;

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

                log.Info("Scheduling User: "+ med.User.FullName +" Medicine: "+ med.Name +" RemindTime: " + med.RemindTime.Value.ToShortTimeString() + " Phone Number: " + med.User.PhoneNumber);
                AddJob(job, trigger);
            }
        }
    }
}