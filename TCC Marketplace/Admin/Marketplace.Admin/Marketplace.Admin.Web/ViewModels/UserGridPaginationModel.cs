using System.Collections.Generic;
using System.ComponentModel;
using Marketplace.Admin.Models;

namespace Marketplace.Admin.ViewModels
{
    public class UserGridPaginationModel :PaginationViewModel
    {
        public List<UserGridViewModel> Users { get; set; }
    }

    public class UserGridViewModel
    {
        public int RowNo { get; set; }
        public int Id { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}