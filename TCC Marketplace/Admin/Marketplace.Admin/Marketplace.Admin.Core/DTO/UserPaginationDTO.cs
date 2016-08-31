using System.Collections.Generic;

namespace Marketplace.Admin.Core.DTO
{
    /// <summary>
    /// Model for user pagination data transfer.
    /// </summary>
    public class UserPaginationDTO :PaginationDTO
    {
        public List<UserListDTO> Users { get; set; }
    }
}
