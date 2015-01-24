using PillBox.Model.Enum;
using System;
using System.Collections.Generic;

namespace PillBox.Model.Entities
{
    public partial class Reminder : IEntityBase
    {
        public virtual int Id { get; set; }
        public virtual Nullable<int> MedicineId { get; set; }
        public virtual Nullable<DateTime> ResponseTime { get; set; }
        public virtual Nullable<DateTime> ReminderSendTime { get; set; }
        public virtual Nullable<DateTime> RemindTimeSent { get; set; }
        public virtual Nullable<bool> IsTaken { get; set; }
        public virtual Nullable<int> SnoozeId { get; set; }
        public virtual string CallSID { get; set; }
        public virtual string MessageSID { get; set; }
        public virtual string Message { get; set; }
        public virtual ReminderType ReminderType { get; set;}
        public virtual Medicine Medicine { get; set; }
        public virtual PillboxUser Patient { get; set; }
    }
}
