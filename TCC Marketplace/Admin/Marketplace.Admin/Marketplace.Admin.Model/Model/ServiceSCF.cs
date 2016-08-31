namespace Marketplace.Admin.Model
{
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Service SCF mapping model
    /// </summary>
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
