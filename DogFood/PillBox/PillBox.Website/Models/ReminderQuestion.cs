using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class ReminderQuestion
    {
        public Reminder Reminder { get; set; }
        public int SurveyResponse { get; set; }
        public List<SurveyResponse> SurveyResponses = new List<SurveyResponse>()
            {
                new SurveyResponse { Id = 1, ResponseValue = "Yes" },
                new SurveyResponse { Id = 2, ResponseValue = "No" },
                new SurveyResponse { Id = 3, ResponseValue = "Snooze" }
            };

        public ReminderQuestion()
        {
            Reminder = new Reminder();
        }

        public int Id
        {
            get;
            set;
        }

        public string MedicineName 
        { 
            get { return Reminder.Medicine.Name; }
        }
    }

    public class SurveyResponse
    {
        public int Id { get; set; }
        public string ResponseValue { get; set; }
    }
}