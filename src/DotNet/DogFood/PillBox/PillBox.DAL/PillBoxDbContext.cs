using Microsoft.AspNet.Identity.EntityFramework;
using PillBox.Core;
using PillBox.DAL.Mapping;
using PillBox.Model.Entities;
using System.Data.Entity;

namespace PillBox.DAL.Entities
{
    public class PillBoxDbContext : IdentityDbContext<PillBoxUser>
    {
        static PillBoxDbContext()
        {
            //Add code you want to always run here
            Database.SetInitializer<PillBoxDbContext>(new PillBoxDbInit());
        }

        public PillBoxDbContext()
            : base(Constants.DB_NAME)
        {
            //Database.SetInitializer<PillBoxContext>(null);
        }

        public PillBoxDbContext(string dbNameOrContext)
            : base(dbNameOrContext)
            //: base("Name=PillBoxContext") Sample code for context selection
        {

        }

        public static PillBoxDbContext Create()
        {
            return new PillBoxDbContext();
        }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MedicineMap());
            modelBuilder.Configurations.Add(new ReminderMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
