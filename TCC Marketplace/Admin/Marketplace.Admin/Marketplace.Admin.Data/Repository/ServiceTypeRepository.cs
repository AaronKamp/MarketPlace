using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{

    /// <summary>
    /// Interface to ServiceTypeRepository.
    /// </summary>
    public interface IServiceTypeRepository : IRepository<ServiceType>
    {
    }

    /// <summary>
    /// Handles database operations for ServiceType entity.
    /// </summary>
    public class ServiceTypeRepository : RepositoryBase<ServiceType>, IServiceTypeRepository
    {
        public ServiceTypeRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
