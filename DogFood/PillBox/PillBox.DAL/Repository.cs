using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using PillBox.Model;

namespace PillBox.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets the uo W.
        /// </summary>
        /// <value>The uo W.</value>
        IUnitOfWork UoW { get; }

        /// <summary>
        /// Adds the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T AddEntity<T>(T entity) where T : class;

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T UpdateEntity<T>(T entity) where T : class;

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        bool DeleteEntity<T>(T entity) where T : class;

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetList<T>() where T : class;

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IQueryable<T> GetList<T>(Expression<Func<T, bool>> query) where T : class;

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        T GetEntity<T>(object primaryKey) where T : class;

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        T GetEntity<T>(Expression<Func<T, bool>> query) where T : class;

    }

    public class Repository : IRepository
    {
        public Repository(IUnitOfWork unitOfWork)
        {
            UoW = unitOfWork;
        }

        #region IRepository Members

        /// <summary>
        /// Adds the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public T AddEntity<T>(T entity) where T : class
        {
            this.UoW.Add(entity);
            return entity;
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public T UpdateEntity<T>(T entity) where T : class
        {
            this.UoW.Update(entity);
            return entity;
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        public bool DeleteEntity<T>(T entity) where T : class
        {
            this.UoW.Delete(entity);
            if (GetEntity<T>(((IEntityBase)entity).Id) != null)
                return false;
            return true;
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> GetList<T>() where T : class
        {
            return GetSession().Set<T>();
        }

        public IQueryable<T> GetList<T>(Expression<Func<T, bool>> query) where T : class
        {
            return GetSession().Set<T>().Where(query);
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        public T GetEntity<T>(object primaryKey) where T : class
        {
            T entity;

            try
            {
                entity = GetSession().Set<T>().Find((primaryKey));
            }
            catch
            {
                entity = null;
            }

            return entity;
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        public T GetEntity<T>(Expression<Func<T, bool>> query) where T : class
        {
            return GetSession().Set<T>().Where(query).FirstOrDefault();
        }

        /// <summary>
        /// Gets the uo W.
        /// </summary>
        /// <value>The uo W.</value>
        public IUnitOfWork UoW { get; private set; }

        #endregion

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns></returns>
        private DbContext GetSession()
        {
            return (DbContext)UoW.Orm;
        }
    }
}
