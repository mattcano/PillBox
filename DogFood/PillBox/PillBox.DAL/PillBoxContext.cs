using PillBox.Core;
using PillBox.DAL.Mapping;
using PillBox.Model.Entities;
using System.Data.Entity;

namespace PillBox.DAL.Entities
{
    public class PillBoxContext : DbContext
    {
        public PillBoxContext()
            : base(Constants.DB_NAME)
        {
            //Database.SetInitializer<PillBoxContext>(null);
        }

        public PillBoxContext(string dbNameOrContext)
            : base(dbNameOrContext)
            //: base("Name=PillBoxContext") Sample code for context selection
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
    }
}
