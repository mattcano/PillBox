using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PillBox.Model.Entities
{
    public partial class PillBoxUser:IdentityUser //:IEntityBase
    {


        public PillBoxUser()
        {
            this.Medicines = new List<Medicine>();
            this.Reminders = new List<Reminder>();
        }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Nullable<bool> IsInTrial { get; set; }
        public virtual Nullable<bool> AutoSendSMS { get; set; }
        public virtual Nullable<bool> AutoSendPhone { get; set; }
        public virtual ICollection<Medicine> Medicines { get; set; }
        public virtual ICollection<Reminder> Reminders { get; set; }
        public virtual string Gender { get; set; }
        public virtual string AgeGroup { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string MedicineTimeList
        {
            get
            {
                string medsList = string.Empty;
                
                foreach(var med in Medicines)
                {
                    medsList += (med.Name + " " + med.RemindTime.GetValueOrDefault().ToShortTimeString() + "<br />");
                }

                return medsList;
            }
        }
    }
}
