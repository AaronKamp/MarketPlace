namespace TCCMarketPlace.Model
{
    public class User 
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public int LocationId { get; set; }
        public int ThermostatId { get; set; }
        public string ZipCode { get; set; }
        public string MacID { get; set; }
    }
}
