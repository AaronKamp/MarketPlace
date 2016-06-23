using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{

    public interface IServiceTypeRepository : IRepository<ServiceType>
    {
    }

    public class ServiceTypeRepository : RepositoryBase<ServiceType>, IServiceTypeRepository
    {
        public ServiceTypeRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
