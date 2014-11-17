using System;
using System.Collections.Generic;

namespace PillBox.Model.Entities
{
    public partial class RemindTime : IEntityBase
    {
        public RemindTime()
        {
            this.Reminders = new List<Reminder>();
        }

        public int Id { get; set; }
        public string RemindValue { get; set; }
        public virtual ICollection<Reminder> Reminders { get; set; }
    }
}
