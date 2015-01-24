using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Services.Interfaces
{
    public interface ITwilioService
    {
        void SendSMS(PillboxUser patient);
        void SendSMS(PillboxUser patient, string message);
        void SendPhoneCall(PillboxUser patient);
        void SendPhoneCall(PillboxUser patient, string message);
        void UpdateResponseDB(int responseId);
    }
}
