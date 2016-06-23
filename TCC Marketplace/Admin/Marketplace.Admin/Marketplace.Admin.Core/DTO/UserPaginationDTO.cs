using System.Collections.Generic;

namespace Marketplace.Admin.Core.DTO
{
    public class UserPaginationDTO :PaginationDTO
    {
        public List<UserListDTO> Users { get; set; }
    }
}
