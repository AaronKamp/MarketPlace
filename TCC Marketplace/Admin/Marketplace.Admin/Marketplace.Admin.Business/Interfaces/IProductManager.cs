using System.Collections.Generic;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    /// <summary>
    /// Product manager interface.
    /// </summary>
    public interface IProductManager
    {
        /// <summary>
        /// Get all product categories.
        /// </summary>
        IList<ProductCategory> GetProductCategories();

        /// <summary>
        /// Get all products in a category.
        /// </summary>
        IList<Product> GetProducts(int categoryId);

        /// <summary>
        /// Get product by Id.
        /// </summary>
        Product GetProduct(int productId);
    }
}
