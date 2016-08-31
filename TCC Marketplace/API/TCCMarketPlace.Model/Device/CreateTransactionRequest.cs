namespace TCCMarketPlace.Model
{
    ///<summary>
    /// Request payload for create transaction api.
    ///</summary>
    public class CreateTransactionRequest
    {
        //Service Id
        public int ServiceId { get; set; }

        //User Id
        public int UserId { get; set; }

        //Thermostat Id
        public int ThermostatId { get; set; }

        /// <summary>
        /// Android or iOS
        /// Android =1, iOS =2
        /// </summary>
        public int DeviceType { get; set; }

        // Product Id
        public string ProductId { get; set; }
    }
}
