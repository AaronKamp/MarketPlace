using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using JWT;
using TCCMarketPlace.Model.Logger;

namespace TCCMarketPlace.Apis
{
    /// <summary>
    /// Authorization Handler.
    /// </summary>
    public class AuthHandler : DelegatingHandler
    {
        const string UnauthorizedErrorMessage = "You don’t have sufficient permission to perform this operation. Kindly contact your system administrator for more details.";
        const string UnhandledErrorMessage = "Something went wrong!!! Please contact your system administrator with below identifier - {0}";

        /// <summary>
        /// Overrides SendAsync method of DelegatingHandler class to implement custom authorization.
        /// </summary>
        /// <param name="request"> Incoming http request. </param>
        /// <param name="cancellationToken"> </param>
        /// <returns>HttpResponseMessage</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                   CancellationToken cancellationToken)
        {
            HttpResponseMessage errorResponse = null;

            try
            {
                IEnumerable<string> authHeaderValues;

                // Try read Authorization header from request
                request.Headers.TryGetValues("Authorization", out authHeaderValues);


                if (authHeaderValues == null)
                    return base.SendAsync(request, cancellationToken);

                //Get the bearer token from Authorization header
                var bearerToken = authHeaderValues.ElementAt(0);
                var token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

                //read jwtSecretKey from application app settings. This key is used to validate the token
                var secret = ConfigurationManager.AppSettings.Get("jwtSecretKey");

                //validate token get user principal
                Thread.CurrentPrincipal = ValidateToken(
                    token,
                    secret,
                    true
                    );

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = Thread.CurrentPrincipal;
                }
            }
            catch (SignatureVerificationException ex)
            {
                new Log4NetLogger().Log(ComposeExceptionLog(request, ex, Guid.NewGuid()), ex, LogLevelEnum.Error);
                errorResponse = request.CreateErrorResponse(HttpStatusCode.Unauthorized, UnauthorizedErrorMessage);
            }
            catch (Exception ex)
            {
                var identifier = Guid.NewGuid();
                new Log4NetLogger().Log(ComposeExceptionLog(request, ex, identifier), ex, LogLevelEnum.Error);
                errorResponse = request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"{ UnhandledErrorMessage}{identifier}");
            }


            return errorResponse != null
                ? Task.FromResult(errorResponse)
                : base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Validate authorization token and user identity.
        /// </summary>
        /// <param name="token"> Token string. </param>
        /// <param name="secret"> JWT secret key </param>
        /// <param name="checkExpiration"> Enable check expiration flag.  </param>
        /// <returns> ClaimsPrincipal </returns>
        private static ClaimsPrincipal ValidateToken(string token, string secret, bool checkExpiration)
        {
            var jsonSerializer = new JavaScriptSerializer();
            var payloadJson = JsonWebToken.Decode(token, secret);
            var payloadData = jsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);


            object exp;
            if (payloadData != null && (checkExpiration && payloadData.TryGetValue("exp", out exp)))
            {
                //check the validity of token
                var validTo = FromUnixTime(long.Parse(exp.ToString()));
                if (DateTime.Compare(validTo, DateTime.UtcNow) <= 0)
                {
                    throw new Exception(
                        string.Format("Token is expired. Expiration: '{0}'. Current: '{1}'", validTo, DateTime.UtcNow));
                }
            }

            var subject = new ClaimsIdentity("Federation", ClaimTypes.Name, ClaimTypes.Role);

            var claims = new List<Claim>();

            if (payloadData != null)
                foreach (var pair in payloadData)
                {
                    var claimType = pair.Key;

                    var source = pair.Value as ArrayList;

                    if (source != null)
                    {
                        claims.AddRange(from object item in source
                                        select new Claim(claimType, item.ToString(), ClaimValueTypes.String));

                        continue;
                    }

                    //read all claims
                    switch (pair.Key)
                    {
                        case "name":
                            claims.Add(new Claim(ClaimTypes.Name, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        case "surname":
                            claims.Add(new Claim(ClaimTypes.Surname, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        case "email":
                            claims.Add(new Claim(ClaimTypes.Email, pair.Value.ToString(), ClaimValueTypes.Email));
                            break;
                        case "role":
                            claims.Add(new Claim(ClaimTypes.Role, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        case "userId":
                            claims.Add(new Claim(ClaimTypes.UserData, pair.Value.ToString(), ClaimValueTypes.Integer));
                            break;
                        case "zipCode":
                            claims.Add(new Claim(ClaimTypes.PostalCode, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        default:
                            var claimValue = pair.Value ?? string.Empty;
                            claims.Add(new Claim(claimType, claimValue.ToString(), ClaimValueTypes.String));
                            break;
                    }
                }

            subject.AddClaims(claims);
            return new ClaimsPrincipal(subject);
        }

        /// <summary>
        /// Converts the expiry timestamp to DateTime
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns>Expiry DateTime</returns>
        private static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Compose informative exception log specific to authorization.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ex"></param>
        /// <param name="execptionIdentifier"></param>
        /// <returns>Detailed Exception Log</returns>
        private static string ComposeExceptionLog(HttpRequestMessage request, Exception ex, Guid execptionIdentifier)
        {
            string exceptionSeperator = "*************************************************************************************************";

            var sbErrorLog = new StringBuilder();
            sbErrorLog.AppendLine();
            sbErrorLog.AppendLine(exceptionSeperator);
            sbErrorLog.AppendLine(execptionIdentifier.ToString());
            sbErrorLog.AppendLine("Error occurred for request- " + request.RequestUri.ToString());
            sbErrorLog.AppendLine("Exception- " + ex.Message);
            sbErrorLog.AppendLine(exceptionSeperator);

            return sbErrorLog.ToString();
        }
    }
}