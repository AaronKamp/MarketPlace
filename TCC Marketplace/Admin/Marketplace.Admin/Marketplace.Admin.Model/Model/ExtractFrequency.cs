namespace Marketplace.Admin.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExtractFrequency")]
    public partial class ExtractFrequency
    {
        public int Id { get; set; }

        public byte FrequencyId { get; set; }

        public DateTime? LastRunDate { get; set; }

        public DateTime? NextRunDate { get; set; }

        public bool? IsLastExecutionSuccess { get; set; }

        public int? TotalFailedExtracts { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string UpdatedUser { get; set; }

        public virtual Frequency Frequency { get; set; }
    }
}
