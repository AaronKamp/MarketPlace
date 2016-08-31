using System.Collections.Generic;

namespace Marketplace.Admin.Core.DTO
{
    /// <summary>
    /// Model for Service pagination object transfer.
    /// </summary>
    public class ServicePaginationDTO : PaginationDTO
    {
        public List<ServiceListDTO> Services { get; set; }        
    }
}
