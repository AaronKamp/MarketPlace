using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Admin.Model
{
    /// <summary>
    /// Deleted Image queue model.
    /// </summary>
    [Table("ImageQueue")]
    public partial class ImageQueue
    {
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public DateTime ActualDeletedDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? PurgedDate { get; set; }

        [Required]
        public string DeletedUser { get; set; }
    }

   
}
