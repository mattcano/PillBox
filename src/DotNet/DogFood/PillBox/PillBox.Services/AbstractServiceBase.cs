using PillBox.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Services
{
    public abstract class AbstractServiceBase
    {
        IRepository _repo;
        IUnitOfWork _unitOfWork;

        public AbstractServiceBase()
        {
            _repo = new Repository(_unitOfWork);
        }

        public IRepository Repo
        {
            get { return _repo; }
        }
    }
}
