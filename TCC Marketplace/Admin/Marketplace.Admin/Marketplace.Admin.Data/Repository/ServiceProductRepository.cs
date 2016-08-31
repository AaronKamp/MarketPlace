using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{

    /// <summary>
    /// Interface to ServiceProductRepository.
    /// </summary>
    public interface IServiceProductRepository : IRepository<ServiceProduct>
    {
    }

    /// <summary>
    /// Handles database operations for ServiceProduct entity.
    /// </summary>
    public class ServiceProductRepository : RepositoryBase<ServiceProduct>, IServiceProductRepository
    {
        public ServiceProductRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
