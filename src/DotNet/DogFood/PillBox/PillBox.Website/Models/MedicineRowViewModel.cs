using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Website.Models
{
    public class MedicineRowViewModel
    {
        private string medName;
        private string remindTimeString;
        private int medId;

        public MedicineRowViewModel(string medName, string remindTimeString, int medId)
        {
            this.medName = medName;
            this.remindTimeString = remindTimeString;
            this.medId = medId;
        }

        public string MedName { get { return medName; } }
        public string RemindTimeString { get { return remindTimeString; } }
        public int MedId { get { return medId; } }
    }
}
