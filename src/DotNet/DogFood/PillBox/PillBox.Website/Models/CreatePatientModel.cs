using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class CreatePatientModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string AgeGroup { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Medicine { get; set; }

        public Nullable<DateTime> RemindTime { get; set; }

        public string Password { get; set; }
    }
}