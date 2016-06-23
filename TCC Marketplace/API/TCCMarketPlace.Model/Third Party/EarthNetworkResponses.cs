using Newtonsoft.Json;

namespace TCCMarketPlace.Model
{
    public class EarthNetworkTokenResponse
    {
        [JsonProperty("OAuth20")]
        public OAuth20 OAuth { get; set; }

    }

    public class OAuth20
    {
        [JsonProperty("access_token")]
        public AccessToken AccessToken { get; set; }
    }
    public class AccessToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

    public class DeleteResponse
    {
        public string Status { get; set; }
        public int ErrorId { get; set; }
        public string Message { get; set; }
        public string CustomerId { get; set; }
    }
}