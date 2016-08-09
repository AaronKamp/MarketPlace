using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TCCMarketPlace.Business.Enum;
using TCCMarketPlace.Business.Interface;
using TCCMarketPlace.Model;
using TCCMarketPlace.Model.TccOAuth;
using TCCMarketPlace.Cache;

namespace TCCMarketPlace.Business
{
    internal class TccAuthentication : IAuthentication
    {
        internal string TccnaBaseAddress => ConfigurationManager.AppSettings["TCC.ApiUrl"];

        private async Task<string> GenerateBearerToken()
        {
            //Auth/Oauth/Token
            var body = "grant_type = client_credentials";

            var tokenApi = $"{TccnaBaseAddress}Auth/Oauth/Token";

            var authorizationHeader = new AuthorizationHeader(AuthorizationScheme.Basic, BasicAuthHeader(), "application/x-www-form-urlencoded");

            var result = await ThirdPartyAPIImplementation.GetBearerToken(tokenApi, body, authorizationHeader);

            return result;
        }

        public async Task<LoginResult> ValidateUser(LoginRequest login)
        {
            //WebApi/api/identity

            var token = await GetBearerToken();

            var param = JsonConvert.SerializeObject(new
            {
                username = login.UserName,
                password = login.Password,
                tenancy = "TCC"
            });


            var identityApi = $"{TccnaBaseAddress}WebApi/api/identity";

            var authorizationHeader = new AuthorizationHeader(AuthorizationScheme.Bearer, token, "application/json");

            var result = await ThirdPartyAPIImplementation.PostValues(identityApi, param, authorizationHeader);

            var loginResult = JsonConvert.DeserializeObject<LoginResult>(result);

            return loginResult;
        }

        private async Task<string> GetBearerToken()
        {
            string tccnaBearerToken = CacheManager.Instance.GetItem<string>("TccnaBearerToken");
            if(string.IsNullOrWhiteSpace(tccnaBearerToken))
            {
                tccnaBearerToken = await GenerateBearerToken();
                CacheManager.Instance.PutItem<string>("TccnaBearerToken", tccnaBearerToken, true);
            }
            return tccnaBearerToken;
        }

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
