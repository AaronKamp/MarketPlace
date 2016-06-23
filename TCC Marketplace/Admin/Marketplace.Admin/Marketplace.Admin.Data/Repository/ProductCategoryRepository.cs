using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{

    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
    }

    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
