using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using JWT;
using TCCMarketPlace.Model;

namespace TCCMarketPlace.Apis.JwToken
{
    /// <summary>
    /// JWT token generation handler
    /// </summary>
    public class TokenGenerator
    {
        /// <summary>
        /// Generates JWT token from Login details.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public string GenerateToken(LoginRequest login)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddHours(2) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


            var payload = new Dictionary<string, object>
            {
                {"email", login.UserName},
                {"userId", login.UserId},
                {"userName", login.UserName},
                {"locationId", login.LocationId},
                {"thermostatId", login.ThermostatId},
                {"zipCode", login.ZipCode},
                {"macID", login.MacID},
                {"role", login.UserType??"TCC"  },
                {"sub", "TCC Mktplc auth"},
                {"nbf", notBefore},
                {"iat", issuedAt},
                {"exp", expiry}
            };

            var apikey = ConfigurationManager.AppSettings.Get("jwtSecretKey");

            var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

            return token;
        }
    }
}