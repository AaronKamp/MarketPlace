using System.Collections.Generic;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    public interface IProductManager
    {
        IList<ProductCategory> GetProductCategories();

        IList<Product> GetProducts(int categoryId);

        Product GetProduct(int productId);
    }
}
