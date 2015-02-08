using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class TrialPatientViewModel
    {
        Patient patient;
        Reminder reminder;

        public TrialPatientViewModel(Patient patient)
        {
            this.patient = patient;
        }

        [Key]
        public int Id { get; set; }

        public Patient Patient
        {
            get { return patient; }
            set { patient = value; }
        }

        public Reminder Reminder
        {
            get { return reminder; }
            set { reminder = value; }
        }
    }
}