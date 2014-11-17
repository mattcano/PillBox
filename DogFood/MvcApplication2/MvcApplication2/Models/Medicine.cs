using System;
using System.Collections.Generic;

namespace MvcApplication2.Models
{
    public partial class Medicine
    {
        public Medicine()
        {
            this.UserMedicineMaps = new List<UserMedicineMap>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserMedicineMap> UserMedicineMaps { get; set; }
    }
}
