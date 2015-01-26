using Ninject.Activation;
using PillBox.DAL;
using PillBox.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace PillBox.Services.DI
{
    class SessionFactoryProvider : Provider<ISessionFactory>
    {
        private static DbContext _context;

        public SessionFactoryProvider()
        {
            _context = new PillBoxDbContext();
        }

        public SessionFactoryProvider(DbContext context)
        {
            _context = context;
        }

        protected override ISessionFactory CreateInstance(IContext context)
        {
            return new SessionFactory(_context);
        }
    }
}
