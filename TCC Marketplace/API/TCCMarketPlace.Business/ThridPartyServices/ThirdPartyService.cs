using System;
using System.Web;
using System.Configuration;
using System.Text;
using TCCMarketPlace.Model;
using TCCMarketPlace.Cache;
using TCCMarketPlace.Business.Interface;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TCCMarketPlace.Business
{
    public class ThirdPartyService : IThirdPartyService
    {
        ServiceProvider _serviceProvider;

        public ThirdPartyService(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private async Task<AccessToken> GetTokenFromAPI()
        {
            var result = await ThirdPartyAPIImplementation.GetValues(GetUrlForAccessToken());
            var response = JsonConvert.DeserializeObject<EarthNetworkTokenResponse>(result);
            return response.OAuth.AccessToken;
        }

        public async Task<bool> UnEnroll(User user, Service service)
        {
            try
            {
                string unEnroll = await ThirdPartyAPIImplementation.GetValues(await GetUrlForUnEnroll(user, service));
                var unEnrollResult = JsonConvert.DeserializeObject<DeleteResponse>(unEnroll);
                if (string.Equals(unEnrollResult.Status, "Cancelled"))
                    return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
        private async Task<string> GetToken(string providerName)
        {
            var providerToken = CacheManager.Instance.GetItem<string>(providerName);
            if (string.IsNullOrWhiteSpace(providerToken))
            {
                var tokenObject = await GetTokenFromAPI();
                providerToken = tokenObject.Token;
                CacheManager.Instance.PutItem<string>(providerName, providerToken, new TimeSpan(0,0,0,tokenObject.ExpiresIn));
            }
            return providerToken;
        }
        private async Task<string> GetUrlForUnEnroll(User user, Service service)
        {
            var accessToken = _serviceProvider.GenerateBearerToken? await GetToken(_serviceProvider.Name):string.Empty;
            StringBuilder strQueryParams = new StringBuilder();
            strQueryParams.Append(_serviceProvider.UnEnrollUrl +(_serviceProvider.UnEnrollUrl.EndsWith("/") ? "unenrolldevicebyprgid?" : "/unenrolldevicebyprgid?"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("prgId"), HttpUtility.UrlEncode(service.PartnerPromoCode) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("customerId"), HttpUtility.UrlEncode(user.UserId.ToString()) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("deviceId"), HttpUtility.UrlEncode(user.MacID.ToString())));
            if (_serviceProvider.GenerateBearerToken)
            {
                strQueryParams.Append("&" + string.Format("{0}={1}", HttpUtility.UrlEncode("access_token"), HttpUtility.UrlEncode(accessToken)));
            }
            return strQueryParams.ToString();
        }

        private string GetUrlForAccessToken()
        {
            var consumerKey = _serviceProvider.AppId;
            var secretKey = _serviceProvider.SecretKey;
            StringBuilder strQueryParams = new StringBuilder();
            strQueryParams.Append(_serviceProvider.TokenUrl + "token?");
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("client_Id"), HttpUtility.UrlEncode(consumerKey) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("client_secret"), HttpUtility.UrlEncode(secretKey) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("grant_type"), HttpUtility.UrlEncode("client_credentials")));
            return strQueryParams.ToString();
        }

        public virtual void Dispose()
        {
        }
    }
}
