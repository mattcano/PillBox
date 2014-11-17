using PillBox.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PillBox.DAL.Mapping
{
    public class UserMedicineMapMap : EntityTypeConfiguration<UserMedicineMap>
    {
        public UserMedicineMapMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserMedicineMap");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.MedicineId).HasColumnName("MedicineId");
            this.Property(t => t.NumberOfPills).HasColumnName("NumberOfPills");
            this.Property(t => t.IsWithFood).HasColumnName("IsWithFood");
            this.Property(t => t.RemindTimeId).HasColumnName("RemindTimeId");

            // Relationships
            this.HasOptional(t => t.Medicine)
                .WithMany(t => t.UserMedicineMaps)
                .HasForeignKey(d => d.MedicineId);
            this.HasOptional(t => t.Patient)
                .WithMany(t => t.UserMedicineMaps)
                .HasForeignKey(d => d.UserId);

        }
    }
}
