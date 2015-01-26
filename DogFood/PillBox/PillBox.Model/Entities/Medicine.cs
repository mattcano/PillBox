using System;
using System.Collections.Generic;

namespace PillBox.Model.Entities
{
    public partial class Medicine : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> RemindTime { get; set; }

        public string UserId { get; set; }
        public PillBoxUser User { get; set; }

    }
}
