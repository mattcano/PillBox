using PillBox.DAL.Entities;
using System.Data.Entity;

namespace PillBox.DAL
{
    public interface ISessionFactory
    {
        /// <summary>
        /// Gets the current uo W.
        /// </summary>
        /// <value>The current uo W.</value>
        IUnitOfWork CurrentUoW { get; }
    }

    public class SessionFactory : ISessionFactory
    {
        private IUnitOfWork uow;
        DbContext _context;

        public SessionFactory()
        {
            _context = new PillBoxDbContext();
        }

        public SessionFactory(DbContext context)
        {

            _context = context;

        }

        #region ISessionFactory Members

        /// <summary>
        /// Gets the current uo W.
        /// </summary>
        /// <value>The current uo W.</value>
        public IUnitOfWork CurrentUoW
        {
            get
            {
                if (uow == null)
                {
                    uow = GetUnitOfWork();
                }

                return uow;
            }
        }

        #endregion

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <returns></returns>
        private IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork(_context);
        }
    }
}
