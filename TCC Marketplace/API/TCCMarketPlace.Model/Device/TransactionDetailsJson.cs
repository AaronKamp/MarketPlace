namespace TCCMarketPlace.Model
{
    /// <summary>
    /// Details of the latest transaction.
    /// </summary>
    public class LatestTransactionDetails
    {
        //Transaction Id
        public int Transaction_Id { get; set; }

        //Service Id
        public int Service_Id { get; set; }

        //User Id
        public int User_Id { get; set; }

        //Thermostat Id
        public int Thermostat_Id { get; set; }
        
        //Status Id
        public int Status_Id { get; set; }
    }

    // Model for JSON deserialization of Transaction Details
    public class TransactionDetails : LatestTransactionDetails
    {
        //Transaction Id in YLM response
        public int User_Transaction_Id { get; set; }

        // In app purchase Id
        public string InApp_Purchase_Id { get; set; }

        /// <summary>
        /// Android or iOS
        /// Android =1, iOS =2
        /// </summary>
        public int Device_Type { get; set; }
    }
}
