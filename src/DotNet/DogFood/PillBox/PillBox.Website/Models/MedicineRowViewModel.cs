using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Website.Models
{
    public class MedicineRowViewModel
    {
        private string p1;
        private string p2;
        private int p3;

        public MedicineRowViewModel(string p1, string p2, int p3)
        {
            // TODO: Complete member initialization
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }
    }
}
