using System.Collections.Generic;
using System.Linq;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    public class ProductManager : IProductManager
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;
        public ProductManager(IProductCategoryRepository productCategoryRepository, IProductRepository productRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
        }

        public Product GetProduct(int productId)
        {
            return _productRepository.GetById(productId);
        }

        public IList<ProductCategory> GetProductCategories()
        {
            return _productCategoryRepository.GetAll().ToList();
        }

        public IList<Product> GetProducts(int categoryId)
        {
            return _productRepository.GetMany(x=>x.ProductCategoryId == categoryId).ToList(); 
        }
    }
}
