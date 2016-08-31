namespace Marketplace.Admin.Model
{
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Service product Mapping Model.
    /// </summary>
    [Table("ServiceProduct")]
    public partial class ServiceProduct
    {
        public int ServiceId { get; set; }

        public int ProductId { get; set; }

        public int Id { get; set; }

        public virtual Product Product { get; set; }

        public virtual Service Service { get; set; }
    }
}
