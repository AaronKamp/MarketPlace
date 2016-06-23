namespace TCCMarketPlace.Model
{
    public class TransactionDetailsResponse 
    {
        public int ServiceId { get; set; }
        public int UserId { get; set; }
        public int TransactionId { get; set; }
        public int ThermostatId { get; set; }
        public string Status { get; set; }
    }
}
