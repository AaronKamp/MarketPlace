namespace Marketplace.Admin.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
