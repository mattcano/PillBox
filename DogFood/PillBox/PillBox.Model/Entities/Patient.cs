using System;
using System.Collections.Generic;

namespace PillBox.Model.Entities
{
    public partial class Patient : IEntityBase
    {
        public Patient()
        {
            this.UserMedicineMaps = new List<UserMedicineMap>();
            this.Reminders = new List<Reminder>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nullable<bool> IsInTrial { get; set; }
        public Nullable<bool> AutoSendSMS { get; set; }
        public Nullable<bool> AutoSendPhone { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<UserMedicineMap> UserMedicineMaps { get; set; }
        public virtual ICollection<Reminder> Reminders { get; set; }
    }
}
