using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class TrialManagerViewModel
    {
        public TrialManagerViewModel()
        {
            Patient = new TrialPatientViewModel();
        }

        public List<TrialPatientViewModel> TrialPatients { get; set; }
        public TrialPatientViewModel Patient { get; set; }
    }
}