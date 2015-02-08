using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class CreateMedicineModel
    {
        public string UserId { get; set; }
        [Required]
        public string MedicineName { get; set; }
        public DateTime? RemindTime { get; set; }
    }
}