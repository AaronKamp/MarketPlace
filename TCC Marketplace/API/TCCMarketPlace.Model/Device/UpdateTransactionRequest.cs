namespace TCCMarketPlace.Model
{
    public class UpdateTransactionRequest
    {
        public int UserId { get; set; }
        public int TransactionId { get; set; }
        public bool PurchaseStatus { get; set; }
        public int ServiceId { get; set; }
        public string InAppTransactionId { get; set; }
        public string PurchaseType { get; set; }
        public decimal PurchaseAmount { get; set; }
        public int ThermostatId { get; set; }

    }
}
