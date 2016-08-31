namespace TCCMarketPlace.Model
{
    /// <summary>
    /// Payload for Update transaction details.
    /// </summary>
    public class UpdateTransactionRequest
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Transaction Id
        /// </summary>
        public int TransactionId { get; set; }

        //Purchase Status
        public bool PurchaseStatus { get; set; }

        //Service Id
        public int ServiceId { get; set; }

        //In app transaction Id.
        public string InAppTransactionId { get; set; }

        //Purchase Type.
        public string PurchaseType { get; set; }

        //Purchase amount.
        public decimal PurchaseAmount { get; set; }

        //Thermostat Id.
        public int ThermostatId { get; set; }

    }
}
