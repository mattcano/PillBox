using System;
using System.Collections.Generic;

namespace PillBox.Model.Entities
{
    public partial class UserMedicineMap : IEntityBase
    {
        public UserMedicineMap()
        {
            this.Reminders = new List<Reminder>();
        }

        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> MedicineId { get; set; }
        public Nullable<int> RemindTimeId { get; set; }
        public virtual Medicine Medicine { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual RemindTime RemindTime { get; set; }
        public virtual ICollection<Reminder> Reminders { get; set; }
        public Nullable<int> NumberOfPills { get; set; }
        public Nullable<bool> IsWithFood { get; set; }
        
    }
}
