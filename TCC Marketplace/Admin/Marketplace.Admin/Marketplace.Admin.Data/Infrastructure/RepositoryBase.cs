using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Marketplace.Admin.Data.Infrastructure
{
    /// <summary>
    /// Repository base class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> where T : class
    {
        #region Properties
        private MarketplaceAdminDb dataContext;
        private readonly IDbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }
        /// <summary>
        /// Database context initialization
        /// </summary>
        protected MarketplaceAdminDb DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region Implementation
        /// <summary>
        /// Add entity to database.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Update an entity in database.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete and entity in database.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Delete entities by expression.
        /// </summary>
        /// <param name="where"> expression.</param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
        }

        /// <summary>
        /// Get an entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> entity</returns>
        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }
        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns> List of entities.</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        /// <summary>
        /// Gets many entities by expression.
        /// </summary>
        /// <param name="where"> Expression.</param>
        /// <returns> Entity list.</returns>
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        /// <summary>
        /// Gets the first entity satisfying an expression
        /// </summary>
        /// <param name="where"> Expression.</param>
        /// <returns>Entity</returns>
        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        #endregion

        /// <summary>
        /// Get all Entities asynchronously.
        /// </summary>
        /// <returns>List of entities.</returns>
        public Task<List<T>> GetAllAsync()
        {
            return dbSet.ToListAsync();
        }

        /// <summary>
        /// Get all Entities asynchronously with a cancellation token.
        /// </summary>
        /// <returns>List of entities.</returns>
        public Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return dbSet.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of entities for a specific page no.
        /// </summary>
        /// <param name="skip"> No. of records to skip.</param>
        /// <param name="take"> No. of records to fetch.</param>
        /// <returns> List of entities.</returns>
        public List<T> PageAll(int skip, int take)
        {
            return dbSet.Skip(skip).Take(take).ToList();
        }

        /// <summary>
        /// Gets a list of entities for a specific page no asynchronously.
        /// </summary>
        /// <param name="skip"> No. of records to skip.</param>
        /// <param name="take"> No. of records to fetch.</param>
        /// <returns> List of entities.</returns>
        public Task<List<T>> PageAllAsync(int skip, int take)
        {
            return dbSet.Skip(skip).Take(take).ToListAsync();
        }
        
        /// <summary>
        /// Gets a list of entities for a specific page no asynchronously.
        /// </summary>
        /// <param name="skip"> No. of records to skip.</param>
        /// <param name="take"> No. of records to fetch.</param>
        /// <returns> List of entities.</returns>
        public Task<List<T>> PageAllAsync(CancellationToken cancellationToken, int skip, int take)
        {
            return dbSet.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Finds an entity by Id.
        /// </summary>
        /// <param name="id"> Entity Id.</param>
        /// <returns> Entity.</returns>
        public T FindById(object id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Finds an entity by Id asynchronously.
        /// </summary>
        /// <param name="id"> Entity Id.</param>
        /// <returns>Entity</returns>
        public Task<T> FindByIdAsync(object id)
        {
            throw new NotImplementedException();// return dbSet.FindAsync(id);
        }

        /// <summary>
        /// Finds an entity by Id asynchronously with cancellation token.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="id"> Entity id </param>
        /// <returns> Entity</returns>
        public Task<T> FindByIdAsync(CancellationToken cancellationToken, object id)
        {
            throw new NotImplementedException(); // return dbSet.FindAsync(cancellationToken, id);
        }

        /// <summary>
        /// Removes an entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public int SaveChanges()
        {
            return dataContext.SaveChanges();
        }

        /// <summary>
        /// Save changes asynchronously.
        /// </summary>
        public Task<int> SaveChangesAsync()
        {
            return dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Save changes asynchronously with a cancellation token.
        /// </summary>
        public Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            return dataContext.SaveChangesAsync(cancellationToken);
        }


    }
}
