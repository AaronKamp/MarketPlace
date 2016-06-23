namespace Marketplace.Admin.Core.DTO
{
    public class PaginationDTO
    {
        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
        public int NoOfPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
