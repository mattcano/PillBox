using System;
using System.Collections.Generic;

namespace MvcApplication2.Models
{
    public partial class RemindTime
    {
        public RemindTime()
        {
            this.Reminders = new List<Reminder>();
            this.UserMedicineMaps = new List<UserMedicineMap>();
        }

        public int Id { get; set; }
        public string RemindValue { get; set; }
        public virtual ICollection<Reminder> Reminders { get; set; }
        public virtual ICollection<UserMedicineMap> UserMedicineMaps { get; set; }
    }
}
