using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Marketplace.Admin.Data.Infrastructure
{
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

        protected MarketplaceAdminDb DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        #endregion

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region Implementation
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
            //dataContext.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
            //dataContext.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
            //dataContext.SaveChanges();
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
            //dataContext.SaveChanges();
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        #endregion


        public Task<List<T>> GetAllAsync()
        {
            return dbSet.ToListAsync();
        }

        public Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return dbSet.ToListAsync(cancellationToken);
        }

        public List<T> PageAll(int skip, int take)
        {
            return dbSet.Skip(skip).Take(take).ToList();
        }

        public Task<List<T>> PageAllAsync(int skip, int take)
        {
            return dbSet.Skip(skip).Take(take).ToListAsync();
        }

        public Task<List<T>> PageAllAsync(CancellationToken cancellationToken, int skip, int take)
        {
            return dbSet.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        public T FindById(object id)
        {
            return dbSet.Find(id);
        }

        public Task<T> FindByIdAsync(object id)
        {
            throw new NotImplementedException();// return dbSet.FindAsync(id);
        }

        public Task<T> FindByIdAsync(CancellationToken cancellationToken, object id)
        {
            throw new NotImplementedException(); // return dbSet.FindAsync(cancellationToken, id);
        }



        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }



        public int SaveChanges()
        {
            return dataContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return dataContext.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            return dataContext.SaveChangesAsync(cancellationToken);
        }


    }
}
