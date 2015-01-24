using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace PillBox.Model.Entities
{
    public partial class PillboxUser:IdentityUser //:IEntityBase
    {
        public PillboxUser()
        {
            this.Medicines = new List<Medicine>();
            this.Reminders = new List<Reminder>();
        }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Nullable<bool> IsInTrial { get; set; }
        public virtual Nullable<bool> AutoSendSMS { get; set; }
        public virtual Nullable<bool> AutoSendPhone { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual ICollection<Medicine> Medicines { get; set; }
        public virtual ICollection<Reminder> Reminders { get; set; }
    }
}
