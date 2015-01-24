using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class TrialPatientViewModel
    {
        PillboxUser patient;
        string reminderMessage;

        public TrialPatientViewModel()
        {

        }

        public TrialPatientViewModel(PillboxUser patient)
            :this()
        {
            this.patient = patient;
        }


        public string PatientName
        {
            get
            {
                return patient.FirstName + " " + patient.LastName;
            }
        }

        public PillboxUser Patient
        {
            get { return patient; }
            set { patient = value; }
        }

        public string Reminder
        {
            get { return reminderMessage; }
            set { reminderMessage = value; }
        }

    }
}