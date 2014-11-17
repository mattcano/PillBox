using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace PillBox.DAL
{

    /// <summary>
    /// The Unit of Work base contract
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets the orm.
        /// </summary>
        /// <value>The orm.</value>
        object Orm { get; }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        void Update<T>(T entity) where T : class;

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        void Delete<T>(T entity) where T : class;
    }

    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="orm">The orm.</param>
        public UnitOfWork(DbContext orm)
        {
            this.Orm = orm;
        }

        #region IUnitOfWork Members

        public object Orm { get; private set; }


        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        public void Add<T>(T entity) where T : class
        {
            try
            {
                ((DbContext)Orm).Set<T>().Add(entity);
                ((DbContext)Orm).SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("An error occured during the Add Entity.\r\n{0}", ex.Message));
            }
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        public void Update<T>(T entity) where T : class
        {
            try
            {
                if (((DbContext)Orm).Entry(entity).State == EntityState.Modified)
                {
                    ((DbContext)Orm).SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("An error occured during the Update Entity.\r\n{0}", ex.Message));
            }
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        public void Delete<T>(T entity) where T : class
        {
            try
            {
                ((DbContext)Orm).Set<T>().Remove(entity);
                ((DbContext)Orm).SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("An error occured during the Delete Entity.\r\n{0}", ex.Message));
            }
        }


        #endregion

        private static string EntitySetName<T>()
        {
            return String.Format(@"{0}s", typeof(T).Name);
        }
    }
}
