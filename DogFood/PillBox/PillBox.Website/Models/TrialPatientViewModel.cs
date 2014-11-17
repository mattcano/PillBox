using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class TrialPatientViewModel
    {
        Patient patient;
        string reminderMessage;
        List<UserMedicineMap> medicines;

        public TrialPatientViewModel()
        {
            medicines = new List<UserMedicineMap>();
        }

        public TrialPatientViewModel(Patient patient)
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

        public Patient Patient
        {
            get { return patient; }
            set { patient = value; }
        }

        public string Reminder
        {
            get { return reminderMessage; }
            set { reminderMessage = value; }
        }

        public List<UserMedicineMap> Medicines
        {
            get { return medicines; }
            set { medicines = value; }
        }
    }
}