using System.Collections.Generic;

namespace Marketplace.Admin.ViewModels
{
    /// <summary>
    /// Product View Model.
    /// </summary>
    public class ProductViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<int> ProductIds { get; set; }
        public string ProductNames { get; set; }
    }
}