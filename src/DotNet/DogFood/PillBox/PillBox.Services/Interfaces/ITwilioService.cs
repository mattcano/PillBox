using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Services.Interfaces
{
    public interface ITwilioService
    {
        void SendSMS(PillBoxUser patient);
        void SendSMS(string userId, int medicineId, string phoneNumber, string message);
        void SendPhoneCall(PillBoxUser patient);
        void SendPhoneCall(PillBoxUser patient, string message);
        void UpdateResponseDB(int responseId);
    }
}
