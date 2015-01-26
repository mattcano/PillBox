using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class CreateMedicineModel
    {
        public string UserId { get; set; }
        public string MedicineName { get; set; }
        public DateTime? RemindTime { get; set; }
    }
}