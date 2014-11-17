using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class Evaluation
    {
        public List<ReminderQuestion> Questions { get; set; }
        public Evaluation()
        {
            Questions = new List<ReminderQuestion>();
        }
    }
}