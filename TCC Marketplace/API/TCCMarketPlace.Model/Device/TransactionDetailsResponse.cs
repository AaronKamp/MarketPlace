namespace TCCMarketPlace.Model
{
    /// <summary>
    /// Response to Get Transaction Details
    /// </summary>
    public class TransactionDetailsResponse 
    {
        //Service Id
        public int ServiceId { get; set; }
        //User Id
        public int UserId { get; set; }
        //Transaction Id
        public int TransactionId { get; set; }
        //Thermostat Id
        public int ThermostatId { get; set; }
        //Transaction Status
        public string Status { get; set; }
    }
}
