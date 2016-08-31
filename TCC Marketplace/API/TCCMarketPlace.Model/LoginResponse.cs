namespace TCCMarketPlace.Model
{
    /// <summary>
    /// Response to Login request.
    /// </summary>
    public class LoginResponse
    {
        //Marketplace URL.
        public string MarketPlaceUrl { get; set; }

        // Login Token.
        public string Token { get; set; }
    }
}
