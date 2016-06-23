using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{

    public interface IProductRepository : IRepository<Product>
    {
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
