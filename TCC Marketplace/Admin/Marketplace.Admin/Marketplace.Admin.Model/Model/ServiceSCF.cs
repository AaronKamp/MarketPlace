namespace Marketplace.Admin.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ServiceSCF")]
    public partial class ServiceSCF
    {
        public int ServiceId { get; set; }

        public int SCFId { get; set; }

        public int Id { get; set; }

        public virtual SCF SCF { get; set; }

        public virtual Service Service { get; set; }
    }
}
