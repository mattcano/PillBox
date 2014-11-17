using Ninject.Activation;
using PillBox.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace PillBox.Services.DI
{
    class UnitOfWorkProvider : Provider<IUnitOfWork>
    {
        private DbContext _context;
        public UnitOfWorkProvider(DbContext context)
        {
            _context = context;
        }

        protected override IUnitOfWork CreateInstance(IContext context)
        {
            return new UnitOfWork(_context);
        }
    }
}
