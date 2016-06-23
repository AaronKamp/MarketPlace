using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{

    public interface IServiceProductRepository : IRepository<ServiceProduct>
    {
    }

    public class ServiceProductRepository : RepositoryBase<ServiceProduct>, IServiceProductRepository
    {
        public ServiceProductRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
