using PillBox.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PillBox.DAL.Mapping
{
    public class RemindTimeMap : EntityTypeConfiguration<RemindTime>
    {
        public RemindTimeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RemindValue)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("RemindTimes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RemindValue).HasColumnName("RemindValue");
        }
    }
}
