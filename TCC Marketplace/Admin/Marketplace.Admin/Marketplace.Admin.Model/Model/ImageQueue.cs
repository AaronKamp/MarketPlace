using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Admin.Model
{
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
