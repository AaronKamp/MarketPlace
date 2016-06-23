namespace Marketplace.Admin.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("State")]
    public partial class State
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public State()
        {
            SCFs = new HashSet<SCF>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string State_Name { get; set; }

        [Required]
        [StringLength(2)]
        public string State_Code { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SCF> SCFs { get; set; }
    }
}
