using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Website.Models
{
    public class WeeklyProgressRowViewModel
    {
        private string p1;
        private int hits;
        private int p2;

        public WeeklyProgressRowViewModel(string p1, int hits, int p2)
        {
            // TODO: Complete member initialization
            this.p1 = p1;
            this.hits = hits;
            this.p2 = p2;
        }
    }
}
