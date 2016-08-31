using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to ProductRepository.
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
    }

    /// <summary>
    /// Handles database operations for Product entity.
    /// </summary>
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
