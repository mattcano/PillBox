using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace MvcApplication2.Models.Mapping
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
            this.Property(t => t.UserMedicineMapId).HasColumnName("UserMedicineMapId");
            this.Property(t => t.RemindTimeId).HasColumnName("RemindTimeId");
            this.Property(t => t.ResponseTime).HasColumnName("ResponseTime");
            this.Property(t => t.IsTaken).HasColumnName("IsTaken");
            this.Property(t => t.SnoozeId).HasColumnName("SnoozeId");

            // Relationships
            this.HasOptional(t => t.RemindTime)
                .WithMany(t => t.Reminders)
                .HasForeignKey(d => d.RemindTimeId);
            this.HasOptional(t => t.UserMedicineMap)
                .WithMany(t => t.Reminders)
                .HasForeignKey(d => d.UserMedicineMapId);

        }
    }
}
