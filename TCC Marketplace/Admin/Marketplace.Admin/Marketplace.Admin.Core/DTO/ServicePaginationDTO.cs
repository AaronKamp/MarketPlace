using System.Collections.Generic;

namespace Marketplace.Admin.Core.DTO
{
    public class ServicePaginationDTO : PaginationDTO
    {
        public List<ServiceListDTO> Services { get; set; }        
    }
}
