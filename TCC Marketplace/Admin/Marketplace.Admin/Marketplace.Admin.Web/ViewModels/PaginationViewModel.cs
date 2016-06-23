namespace Marketplace.Admin.ViewModels
{
    public class PaginationViewModel
    {
        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
        public int NoOfPages { get; set; }
        public int CurrentPage { get; set; }
    }
}