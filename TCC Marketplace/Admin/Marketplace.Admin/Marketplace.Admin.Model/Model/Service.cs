namespace Marketplace.Admin.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Service")]
    public partial class Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            ServiceProducts = new HashSet<ServiceProduct>();
            SCFs = new HashSet<SCF>();
        }

        public int Id { get; set; }

        public byte ServiceTypeId { get; set; }

        [Required]
        [StringLength(255)]
        public string Tilte { get; set; }

        [Required]
        [StringLength(1000)]
        public string ShortDescription { get; set; }

        [StringLength(5000)]
        public string LongDescription { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(255)]
        public string URL { get; set; }

        [StringLength(255)]
        public string PartnerPromoCode { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [StringLength(255)]
        public string IconImage { get; set; }

        [Required]
        [StringLength(255)]
        public string SliderImage { get; set; }

        [StringLength(255)]
        public string CustomField1 { get; set; }

        [StringLength(255)]
        public string CustomField2 { get; set; }

        [StringLength(255)]
        public string CustomField3 { get; set; }

        public bool MakeLive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [Required]
        [StringLength(255)]
        public string UpdatedUser { get; set; }

        public DateTime? LastExtractedDate { get; set; }

        [StringLength(256)]
        public string InAppPurchaseId { get; set; }

        public decimal PurchasePrice { get; set; }

        public string ThirdPartyAPI { get; set; }

        public bool ServiceStatusAPIAvailable { get; set; }

        [StringLength(2000)]
        public string ZipCodes { get; set; }

        public bool DisableAPIAvailable { get; set; }

        public virtual ServiceType ServiceType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceProduct> ServiceProducts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SCF> SCFs { get; set; }
    }
}
