using System;
using System.Collections.Generic;

namespace MvcApplication2.Models
{
    public partial class UserMedicineMap
    {
        public UserMedicineMap()
        {
            this.Reminders = new List<Reminder>();
        }

        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> MedicineId { get; set; }
        public Nullable<int> RemindTimeId { get; set; }
        public Nullable<int> NumberOfPills { get; set; }
        public Nullable<bool> IsWithFood { get; set; }
        public virtual Medicine Medicine { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<Reminder> Reminders { get; set; }
        public virtual RemindTime RemindTime { get; set; }
    }
}
