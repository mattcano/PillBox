using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Website.Models
{
    public class WeeklyProgressRowViewModel
    {
        private string medName;
        private int hits;
        private int totalForWeek;

        public WeeklyProgressRowViewModel(string medName, int hits, int totalForWeek)
        {
            this.medName = medName;
            this.hits = hits;
            this.totalForWeek = totalForWeek;
        }

        public string MedName { get { return medName; } }
        public int Hits { get { return hits; } }
        public int TotalForWeek { get { return totalForWeek; } }
    }
}
