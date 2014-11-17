using System;
using System.Collections.Generic;

namespace PillBox.Model.Entities
{
    public partial class Medicine : IEntityBase
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
