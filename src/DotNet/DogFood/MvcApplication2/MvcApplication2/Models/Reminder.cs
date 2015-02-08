using System;
using System.Collections.Generic;

namespace MvcApplication2.Models
{
    public partial class Reminder
    {
        public int Id { get; set; }
        public Nullable<int> UserMedicineMapId { get; set; }
        public Nullable<int> RemindTimeId { get; set; }
        public Nullable<System.DateTime> ResponseTime { get; set; }
        public Nullable<System.DateTime> RemindSendTime { get; set; }
        public Nullable<bool> IsTaken { get; set; }
        public Nullable<int> SnoozeId { get; set; }
        public string Message { get; set; }
        public ReminderType ReminderType { get; set; }
        public virtual RemindTime RemindTime { get; set; }
        public virtual UserMedicineMap UserMedicineMap { get; set; }
    }

    public enum ReminderType
    {
        EMAIL,
        SMS,
        TEXT
    }
}
