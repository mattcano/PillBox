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

        public static void Start()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        public static void AddJob(IJobDetail job, ITrigger trigger)
        {
            scheduler.ScheduleJob(job, trigger);
        }

        public static void ScheduleUpcomingJobs()
        {

        }
    }
}