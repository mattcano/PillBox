using PillBox.Model.Entities;
using PillBox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Services
{
    public class TwilioService:ITwilioService
    {

        public void SendSMS(Patient patient)
        {
            throw new NotImplementedException();
        }

        public void SendSMS(Patient patient, string message)
        {
            throw new NotImplementedException();
        }

        public void UpdateResponseDB(int responseId)
        {
            throw new NotImplementedException();
        }


        public void SendPhoneCall(Patient patient)
        {
            throw new NotImplementedException();
        }

        public void SendPhoneCall(Patient patient, string message)
        {
            throw new NotImplementedException();
        }
    }
}
