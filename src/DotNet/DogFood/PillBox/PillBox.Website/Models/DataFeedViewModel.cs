using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class DataFeedViewModel
    {
        Reminder _reminder;

        public DataFeedViewModel(Reminder reminder)
        {
            _reminder = reminder;
        }

        public string FirstName
        {
            get
            {
                try { return _reminder.User.FirstName; }
                catch { return "NULL"; }
            }
        }

        public string LastName
        {
            get
            {
                try { return _reminder.User.LastName; }
                catch { return "NULL"; }
            }
        }

        public string Gender
        {
            get
            {
                try { return _reminder.User.Gender; }
                catch { return "NULL"; }
            }
        }

        public string AgeGroup
        {
            get
            {
                try { return _reminder.User.AgeGroup; }
                catch { return "NULL"; }
            }
        }

        public string MedicineName
        {
            get
            {
                try { return _reminder.Medicine.Name; }
                catch { return "NULL"; }
            }
        }

        public string RemindSentDate
        {
            get
            {
                try { return _reminder.RemindTimeSent.Value.ToShortDateString(); }
                catch { return "NULL"; }
            }
        }

        public string ReminderSendTime
        {
            get
            {
                try { return _reminder.Medicine.RemindTime.Value.ToShortTimeString(); }
                catch { return "NULL"; }
            }
        }

        public string ResponseDateTime
        {
            get
            {
                try
                {
                    return _reminder.ResponseTime.ToString();
                }
                catch
                {
                    return "NULL";
                }
            }
        }

        public string IsTaken
        {
            get
            {
                try
                {
                    return _reminder.IsTaken.ToString();
                }
                catch
                {
                    return "NULL";
                }
            }
        }

        public string ResponseMessage
        {
            get
            { 
                try { return _reminder.Message; }
                catch { return "NULL"; }
            }
        }

        public string IsLateResponse
        {
            get
            {
                try
                {
                    if (_reminder.Message.Contains("@LATE"))
                        return "TRUE";
                    return "FALSE";
                }
                catch
                {
                    return "";
                }
            }
        }
    }
}