using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TCCMarketPlace.Business.Enum;
using TCCMarketPlace.Business.Interface;
using TCCMarketPlace.Model;
using TCCMarketPlace.Model.TccOAuth;
using TCCMarketPlace.Cache;
using TCCMarketPlace.Model.Logger;

namespace TCCMarketPlace.Business
{
    /// <summary>
    /// Class to handle TCC user authentication 
    /// </summary>
    internal class TccAuthentication : IAuthentication
    {

        /// <summary>
        /// Generates marketplace app authentication token. 
        /// </summary>
        /// <returns> Bearer token.</returns>
        private async Task<string> GenerateBearerToken()
        {
     
            var body = "grant_type = client_credentials";

            var tokenApi = ConfigurationManager.AppSettings["TCC.AuthApiUrl"];

            var authorizationHeader = new AuthorizationHeader(AuthorizationScheme.Basic, BasicAuthHeader(), "application/x-www-form-urlencoded");

            var result = await ThirdPartyAPIImplementation.GetBearerToken(tokenApi, body, authorizationHeader);

            return result;
            
        }
        /// <summary>
        /// Validates user data.
        /// </summary>
        /// <param name="login" cref="LoginRequest"> </param>
        /// <returns>LoginResult</returns>
        public async Task<LoginResult> ValidateUser(LoginRequest login)
        {
           
            var token = await GetBearerToken();

            var param = JsonConvert.SerializeObject(new
            {
                username = login.UserName,
                password = login.Password,
                tenancy = "TCC"
            });


            var identityApi = ConfigurationManager.AppSettings["TCC.IdentityApiUrl"];

            var authorizationHeader = new AuthorizationHeader(AuthorizationScheme.Bearer, token, "application/json");

            var result = await ThirdPartyAPIImplementation.PostValues(identityApi, param, authorizationHeader);

            var loginResult = JsonConvert.DeserializeObject<LoginResult>(result);

            return loginResult;
        }
        /// <summary>
        /// Get the TCC Authentication bearer token from Redis cache or generate new.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetBearerToken()
        {
            string tccnaBearerToken = string.Empty;
            try {
                tccnaBearerToken = CacheManager.Instance.GetItem<string>("TccnaBearerToken");
                if (string.IsNullOrWhiteSpace(tccnaBearerToken))
                {
                    tccnaBearerToken = await GenerateBearerToken();
                    CacheManager.Instance.PutItem<string>("TccnaBearerToken", tccnaBearerToken, true);
                }
            }
            catch (RedisCacheException ex)
            {
                if (string.IsNullOrWhiteSpace(tccnaBearerToken))
                {
                    tccnaBearerToken = await GenerateBearerToken();
                }

                //Redis cache exception detected and logged as warning. 

                var exceptionIdentifier = Guid.NewGuid();
                new Log4NetLogger().Log(LogHelper.ComposeExceptionLog(ex.ExceptionMessage, exceptionIdentifier), ex.RedisException, LogLevelEnum.Warning);
            }
            catch
            {
                throw;
            }
            return tccnaBearerToken;

        }

        /// <summary>
        /// Generate basic authorization header info.
        /// </summary>
        /// <returns></returns>
        private string BasicAuthHeader()
        {
            var authInfo = ConfigurationManager.AppSettings["TCC.AppId"] + ":" + ConfigurationManager.AppSettings["TCC.Secret_Key"];
            var byteArray = Encoding.ASCII.GetBytes(authInfo);
            return Convert.ToBase64String(byteArray);
        }

        public virtual void Dispose()
        {
        }
    }
}
