using PillBox.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PillBox.DAL.Mapping
{
    public class ReminderMap : EntityTypeConfiguration<Reminder>
    {
        public ReminderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Reminders");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ResponseTime).HasColumnName("ResponseTime");
            this.Property(t => t.IsTaken).HasColumnName("IsTaken");
            this.Property(t => t.SnoozeId).HasColumnName("SnoozeId");

            // Relationships
        }
    }
}
