using PillBox.Model.Enum;
using System;
using System.Collections.Generic;

namespace PillBox.Model.Entities
{
    public partial class Reminder : IEntityBase
    {
        public int Id { get; set; }
        public Nullable<int> MedicineId { get; set; }
        public Nullable<int> RemindTimeId { get; set; }
        public Nullable<System.DateTime> ResponseTime { get; set; }
        public string RemindSendTime { get; set; }
        public Nullable<DateTime> RemindTimeSent { get; set; }
        public Nullable<bool> IsTaken { get; set; }
        public Nullable<int> SnoozeId { get; set; }
        public string CallSID { get; set; }
        public string MessageSID { get; set; }
        public string Message { get; set; }
        public ReminderType ReminderType { get; set;}
        public virtual RemindTime RemindTime { get; set; }
        public virtual Medicine Medicine { get; set; }
        public virtual PillboxUser Patient { get; set; }
    }
}
