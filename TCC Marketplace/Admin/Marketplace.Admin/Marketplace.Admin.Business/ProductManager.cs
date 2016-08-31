using System.Collections.Generic;
using System.Linq;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    /// <summary>
    /// Handles product functionalities.
    /// </summary>
    public class ProductManager : IProductManager
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="productCategoryRepository"></param>
        /// <param name="productRepository"></param>
        public ProductManager(IProductCategoryRepository productCategoryRepository, IProductRepository productRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Get product by Id.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Product</returns>
        public Product GetProduct(int productId)
        {
            return _productRepository.GetById(productId);
        }

        /// <summary>
        /// Get all product categories.
        /// </summary>
        /// <returns> Product category list.</returns>
        public IList<ProductCategory> GetProductCategories()
        {
            return _productCategoryRepository.GetAll().ToList();
        }

        /// <summary>
        /// Get all products in a category.
        /// </summary>
        /// <param name="categoryId"> </param>
        /// <returns> Product list in a category.</returns>
        public IList<Product> GetProducts(int categoryId)
        {
            return _productRepository.GetMany(x=> x.ProductCategoryId == categoryId).ToList(); 
        }
    }
}
