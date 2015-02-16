using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PillBox.Services;
using PillBox.Model.Entities;
using PillBox.Website.ScheduledTasks;

namespace PillBox.IntegrationTest
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void Can_Send_Email_On_Timer()
        {
        }

        [TestMethod]
        public void Can_Send_And_Breakup_Large_Text_Messages_And_Have_Them_Arrive_In_Sent_Order()
        {
            // Arrange
            TwilioService service = new TwilioService();

            string phoneNumber = "3014373223";
            string longMessage = "Hello! This is a PillBox reminder to take your " 
                + "A name of a super long medicine? Maybe like Advil, Multi, Oil, Biotin, Vitamin B? Is this long enough? Probably not lets make it longer!"
                + ". Reply Y once you've done so.";

            // Act
            service.SendSMS(null,0, phoneNumber, longMessage);

            // Assert
        }

        [TestMethod]
        public void Can_Schedule_And_Unschedule_Reminders()
        {
            // Arrange
            DateTime currentTime = DateTime.Now;
            string phoneNumber = "3014373223";
            string userId = "damolaomotosho";
            PillBoxUser user = new PillBoxUser() { Id = userId, PhoneNumber = phoneNumber };

            Medicine med = new Medicine();
            med.Id = 123;
            med.Name = "Advil";
            med.UserId = user.Id;
            med.User = user;
            med.RemindTime = currentTime;

            DateTime currentTime2 = DateTime.Now;
            Medicine med2 = new Medicine();
            med2.Id = 1234;
            med2.Name = "Tyelonol";
            med2.UserId = user.Id;
            med2.User = user;
            med2.RemindTime = currentTime2;

            // Act
            JobScheduler.ScheduleMedicineReminder(med);
            JobScheduler.ScheduleMedicineReminder(med2);

            // Assert
            Assert.IsTrue(JobScheduler.NumberOfJobs == 2);
            JobScheduler.RemoveJob(med2.Id);
            Assert.IsTrue(JobScheduler.NumberOfJobs == 1);
            Assert.IsTrue(Int32.Parse(JobScheduler.Jobs[0].ItemArray[1].ToString()) == med.Id);
        }
    }
}
