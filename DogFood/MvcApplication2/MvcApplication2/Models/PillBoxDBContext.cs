using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MvcApplication2.Models.Mapping;

namespace MvcApplication2.Models
{
    public partial class PillBoxDBContext : DbContext
    {
        static PillBoxDBContext()
        {
            Database.SetInitializer<PillBoxDBContext>(null);
        }

        public PillBoxDBContext()
            : base("Name=PillBoxDBContext")
        {
        }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<RemindTime> RemindTimes { get; set; }
        public DbSet<UserMedicineMap> UserMedicineMaps { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MedicineMap());
            modelBuilder.Configurations.Add(new PatientMap());
            modelBuilder.Configurations.Add(new ReminderMap());
            modelBuilder.Configurations.Add(new RemindTimeMap());
            modelBuilder.Configurations.Add(new UserMedicineMapMap());
        }

        public DbSet<TrialPatientViewModel> TrialPatientViewModels { get; set; }
    }
}
