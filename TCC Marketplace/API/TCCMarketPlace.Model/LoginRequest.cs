namespace TCCMarketPlace.Model
{
    /// <summary>
    /// Login Request Model.
    /// </summary>
    public class LoginRequest 
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int LocationId { get; set; }
        public int ThermostatId { get; set; }
        public string ZipCode { get; set; }
        public string UserType { get; set; }
        public string MacID { get; set; }
    }
}
