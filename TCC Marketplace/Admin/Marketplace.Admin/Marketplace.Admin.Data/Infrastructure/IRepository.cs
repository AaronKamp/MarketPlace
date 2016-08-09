using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Marketplace.Admin.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        // Marks an entity as new
        void Add(T entity);
        
        // Marks an entity as modified
        void Update(T entity);
       
        // Marks an entity to be removed
        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);
        
        // Get an entity by int id
        T GetById(int id);
        
        // Get an entity using delegate
        T Get(Expression<Func<T, bool>> where);
        
        // Gets all entities of type T
        IEnumerable<T> GetAll();
        
        // Gets entities using delegate
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
        
         List<T> PageAll(int skip, int take);
        
        Task<List<T>> PageAllAsync(int skip, int take);
        
        Task<List<T>> PageAllAsync(CancellationToken cancellationToken, int skip, int take);
        
        T FindById(object id);
        
        Task<T> FindByIdAsync(object id);

        Task<T> FindByIdAsync(CancellationToken cancellationToken, object id);

        void Remove(T entity);
        
        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken);

    }
}
