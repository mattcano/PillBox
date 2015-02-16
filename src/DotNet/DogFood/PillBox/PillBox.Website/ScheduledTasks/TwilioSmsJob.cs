using PillBox.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.ScheduledTasks
{
    public class TwilioSmsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;

            JobDataMap dataMap = context.Trigger.JobDataMap;

            string userId = dataMap.GetString("userId");
            int medicineId = dataMap.GetInt("medicineId");
            string phoneNumber = dataMap.GetString("phoneNumber");
            string medicine = dataMap.GetString("medicine");

            string reminderMessage =
                "Hello! This is a PillBox reminder to take your " + medicine + ". Reply Y once you've done so.";

            TwilioService twilioService = new TwilioService();
            twilioService.SendSMS(userId, medicineId, phoneNumber, reminderMessage);
        }
    }
}