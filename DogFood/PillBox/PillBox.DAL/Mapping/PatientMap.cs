using PillBox.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PillBox.DAL.Mapping
{
    public class PatientMap : EntityTypeConfiguration<Patient>
    {
        public PatientMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FirstName)
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Patients");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.Email).HasColumnName("Email");
        }
    }
}
