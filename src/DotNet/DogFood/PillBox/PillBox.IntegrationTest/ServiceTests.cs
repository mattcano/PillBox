using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PillBox.Services;

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
    }
}
