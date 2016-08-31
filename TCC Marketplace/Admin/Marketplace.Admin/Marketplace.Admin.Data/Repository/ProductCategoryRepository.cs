using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to ProductCategoryRepository.
    /// </summary>
    
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
    }

    /// <summary>
    /// Handles database operations for ProductCategory entity.
    /// </summary>
    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
