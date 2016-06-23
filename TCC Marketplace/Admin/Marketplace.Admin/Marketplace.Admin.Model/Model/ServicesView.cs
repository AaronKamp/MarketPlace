namespace Marketplace.Admin.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ServicesView")]
    public partial class ServicesView
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string Tilte { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1000)]
        public string ShortDescription { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(256)]
        public string InAppPurchaseId { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(255)]
        public string IconImage { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime StartDate { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime EndDate { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(255)]
        public string URL { get; set; }

        [Key]
        [Column(Order = 8)]
        public bool IsActive { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(50)]
        public string ServiceType { get; set; }

        [StringLength(2000)]
        public string ZipCodes { get; set; }

        public string States { get; set; }

        public string Countries { get; set; }

        public string SCFCodes { get; set; }

        public string Thermostats { get; set; }

       public DateTime UpdatedDate { get; set; }
    }
}
