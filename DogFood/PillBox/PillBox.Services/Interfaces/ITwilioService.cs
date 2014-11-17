using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Services.Interfaces
{
    public interface ITwilioService
    {
        void SendSMS(Patient patient);
        void SendSMS(Patient patient, string message);
        void SendPhoneCall(Patient patient);
        void SendPhoneCall(Patient patient, string message);
        void UpdateResponseDB(int responseId);
    }
}
