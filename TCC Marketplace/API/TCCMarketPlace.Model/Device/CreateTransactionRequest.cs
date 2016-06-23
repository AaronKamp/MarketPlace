namespace TCCMarketPlace.Model
{
    public class CreateTransactionRequest
    {
        public int ServiceId { get; set; }
        public int UserId { get; set; }
        public int ThermostatId { get; set; }
        public int DeviceType { get; set; }
        public string ProductId { get; set; }
    }
}
