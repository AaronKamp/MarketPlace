using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Marketplace.Admin.Data.Infrastructure
{
    /// <summary>
    /// Contract for all repositories
    /// </summary>
    /// <typeparam name="T"> Object type </typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Marks an entity as new
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        ///  Marks an entity as modified
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        ///  Marks an entity to be removed
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Marks an entity to be removed by condition
        /// </summary>
        /// <param name="where"></param>
        void Delete(Expression<Func<T, bool>> where);

        /// <summary>
        /// Get an entity by integer id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Object</returns>
        T GetById(int id);

        /// <summary>
        ///  Get an entity using condition
        /// </summary>
        /// <param name="where"> Expression</param>
        /// <returns> Object</returns>
        T Get(Expression<Func<T, bool>> where);

        /// <summary>
        /// Gets all entities of type T
        /// </summary>
        /// <returns> List of objects</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets entities using condition
        /// </summary>
        /// <param name="where"> Expression</param>
        /// <returns> List of Object</returns>
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        /// <summary>
        /// Gets all entities of type T
        /// </summary>
        /// <returns> List of objects</returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Get all Entities asynchronously with a cancellation token.
        /// </summary>
        /// <returns>List of entities.</returns>
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of entities for a specific page no.
        /// </summary>
        /// <param name="skip"> No. of records to skip.</param>
        /// <param name="take"> No. of records to fetch.</param>
        /// <returns> List of entities.</returns>
        List<T> PageAll(int skip, int take);

        /// <summary>
        /// Gets a list of entities for a specific page no asynchronously.
        /// </summary>
        /// <param name="skip"> No. of records to skip.</param>
        /// <param name="take"> No. of records to fetch.</param>
        /// <returns> List of entities.</returns>
        Task<List<T>> PageAllAsync(int skip, int take);

        /// <summary>
        /// Gets a list of entities for a specific page no asynchronously.
        /// </summary>
        /// <param name="skip"> No. of records to skip.</param>
        /// <param name="take"> No. of records to fetch.</param>
        /// <returns> List of entities.</returns>
        Task<List<T>> PageAllAsync(CancellationToken cancellationToken, int skip, int take);

        /// <summary>
        /// Finds an entity by Id.
        /// </summary>
        /// <param name="id"> Entity Id.</param>
        /// <returns> Entity.</returns>
        T FindById(object id);

        /// <summary>
        /// Finds an entity by Id asynchronously.
        /// </summary>
        /// <param name="id"> Entity Id.</param>
        /// <returns>Entity</returns>
        Task<T> FindByIdAsync(object id);

        /// <summary>
        /// Finds an entity by Id asynchronously with cancellation token.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="id"> Entity id </param>
        /// <returns> Entity</returns>
        Task<T> FindByIdAsync(CancellationToken cancellationToken, object id);

        /// <summary>
        /// Removes an entity.
        /// </summary>
        /// <param name="entity"></param>
        void Remove(T entity);

        /// <summary>
        /// Save changes
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Save changes asynchronously.
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Save changes asynchronously with a cancellation token.
        /// </summary>
        Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken);

    }
}
