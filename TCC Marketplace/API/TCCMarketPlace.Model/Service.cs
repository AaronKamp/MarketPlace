namespace TCCMarketPlace.Model
{
    /// <summary>
    /// Service details business model.
    /// </summary>
    public class Service
    {
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageUrl { get; set; }
        public bool IsBought { get; set; }
        public bool IsEnabled { get; set; }
        public string ReportUrl { get; set; }
        public string SignUpUrl { get; set; }
        public string ServiceProviderId { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public int ThermostatId { get; set; }
        public string PartnerPromoCode { get; set; }
        public bool IsDisableApiAvailable { get; set; }
    }
}
