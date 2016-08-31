namespace Marketplace.Admin.Core.DTO
{
    /// <summary>
    /// General pagination details object to inherit to any object's pagination model.
    /// </summary>
    public class PaginationDTO
    {
        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
        public int NoOfPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
