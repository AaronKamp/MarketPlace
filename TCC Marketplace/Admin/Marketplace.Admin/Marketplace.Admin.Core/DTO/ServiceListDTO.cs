using System;

namespace Marketplace.Admin.Core.DTO
{
    /// <summary>
    /// Service List Data transfer object.
    /// </summary>
    public class ServiceListDTO
    {
        public int ServiceId { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Type { get; set; }

        public string ProductId { get; set; }

        public string Countries { get; set; }

        public string States { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public string IconImage { get; set; }

        public DateTime UpdatedDate { get; set; }

    }
}
