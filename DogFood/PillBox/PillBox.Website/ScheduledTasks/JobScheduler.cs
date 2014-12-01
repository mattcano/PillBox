using PillBox.DAL.Entities;
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
            scheduler.Start();
        }

        public static void AddJob(IJobDetail job, ITrigger trigger)
        {
            scheduler.ScheduleJob(job, trigger);
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
    }
}