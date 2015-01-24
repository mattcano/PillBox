using Ninject.Modules;
using PillBox.DAL;
using PillBox.DAL.Entities;
using System.Data.Entity;

namespace PillBox.Services.DI
{
    public class DICoreModule : NinjectModule
    {
                private DbContext _context;

        public DICoreModule()
        {
            _context = new PillBoxContext();
        }

        public DICoreModule(string dbName)
        {
            _context = new PillBoxContext(dbName);
        }

        public override void Load()
        {
            Bind<IRepository>().To<Repository>();
            Bind<ISessionFactory>().ToProvider(new SessionFactoryProvider(_context));
            Bind<IUnitOfWork>().ToProvider(new UnitOfWorkProvider(_context));
            Bind<IMedicineService>().To<MedicineService>();
        }
    }
}
