namespace TCCMarketPlace.Model
{
    /// <summary>
    /// In app purchase status
    /// </summary>
    public class InAppPurchaseStatus
    {
        // Transaction Id
        public int TransactionId { get; set; }
        // Purchase status.
        public bool PurchaseStatus { get; set; }
    }
}
