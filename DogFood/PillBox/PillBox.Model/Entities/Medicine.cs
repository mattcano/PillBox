using System;
using System.Collections.Generic;

namespace PillBox.Model.Entities
{
    public partial class Medicine : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RemindTime { get; set; }

        public Guid UserId { get; set; }
        public PillboxUser User { get; set; }

    }
}
