using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using JWT;
using TCCMarketPlace.Apis.Models;
using TCCMarketPlace.Business;
using TCCMarketPlace.Model;

namespace TCCMarketPlace.Apis.Controllers
{
    [Authorize]
    [RoutePrefix("api/MarketPlace")]
    public class MarketPlaceController : BaseController
    {
        [Route("GetMarketPlaceList/{typeId}")]
        public async Task<ApiResponse<MarketPlaceDetails>> GetMarketPlaceList(int typeId, [FromUri] FilterRequest request)
        {
            ApiResponse<MarketPlaceDetails> response = new ApiResponse<MarketPlaceDetails>();

            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                var result = await marketPlace.GetMarketPlaceList(CurrentUser, typeId, request.SearchKey);
                response.Data = result;
            }
            return response;
        }

        [Route("GetNewlyAddedServices/{typeId}")]
        public async Task<ApiResponse<MarketPlaceDetails>> GetNewlyAddedServices(int typeId, [FromUri] FilterRequest request)
        {
            ApiResponse<MarketPlaceDetails> response = new ApiResponse<MarketPlaceDetails>();

            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketPlace.GetNewlyAddedServices(CurrentUser, typeId, request.SearchKey);
            }

            return response;
        }

        [Route("GetMyServices/{typeId}")]
        public async Task<ApiResponse<MarketPlaceDetails>> GetMyServices(int typeId, [FromUri] FilterRequest request)
        {
            ApiResponse<MarketPlaceDetails> response = new ApiResponse<MarketPlaceDetails>();

            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketPlace.GetMyServices(CurrentUser, typeId, request.SearchKey);
            }

            return response;
        }

        [Route("GetCategories/{typeId}")]
        public ApiResponse<List<Category>> GetCategories(int typeId)
        {
            ApiResponse<List<Category>> response = new ApiResponse<List<Category>>();

            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = marketPlace.GetCategories(string.Empty, typeId);
            }

            return response;
        }

        [Route("GetDetails/{serviceId}")]
        public async Task<ApiResponse<Service>> GetDetails(int serviceId  )
        {
            ApiResponse<Service> response = new ApiResponse<Service>();

            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketPlace.GetDetails(CurrentUser, serviceId);
            }

            return response;
        }

        [Route("GetSlideShowImages/{typeId}")]
        public async Task<ApiResponse<object>> GetSlideShowImages(int typeId)
        {
            ApiResponse<object> response = new ApiResponse<object>();

            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketPlace.GetSlideShowImages(CurrentUser, typeId);
            }

            return response;
        }
       
        [Route("EnableOrDisableService")]
        [HttpPost]
        public async Task<ApiResponse<Service>> EnableOrDisableService([FromBody] Service service)
        {
            ApiResponse<Service> response = new ApiResponse<Service>();
            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketPlace.EnableOrDisableService(CurrentUser, service);
            }
            return response;
        }

        [Route("RemoveService")]
        [HttpPost]
        public async Task<ApiResponse<string>> RemoveAddOn([FromBody] Service service)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketPlace.RemoveService(CurrentUser, service);
            }
            return response;
        }

        [Route("SaveReportUrl")]
        [HttpPost]
        public async Task<ApiResponse<string>> SaveReportUrl([FromBody] Service service)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            using (var marketplace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketplace.SaveReportUrl(CurrentUser, service);
            }
            return response;
        }

        [Route("SubscribeToService")]
        [HttpPost]
        public async Task<ApiResponse<string>> SubscribeToService([FromBody] Service service)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketPlace.SubscribeToService(CurrentUser, service);
            }
            return response;
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequest login)
        {
            // add code for user authentication
            ApiResponse<LoginResponse> response = new ApiResponse<LoginResponse>();

            var isAuthenticated = await AuthenticateUser(login);
            if (isAuthenticated)
            {
                string hostedurl = ConfigurationManager.AppSettings["MarketPlaceUrl"];

                var token = CreateToken(login);

                var landingPageUrl = $"{hostedurl}#/UserAuth/{GetJSonUser(login)}/{token}";

                var responseData = new LoginResponse
                {
                    MarketPlaceUrl = landingPageUrl,
                    Token = token
                };

                response.Data = responseData;
                response.Status = "SUCCESS";
            }
            else
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
            }
            return Ok(response);
        }

        private string CreateToken(LoginRequest login)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddHours(2) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


            var payload = new Dictionary<string, object>
            {
                {"email", login.UserName},
                {"userId", login.UserId},
                {"role", "TCC_User"  },
                {"sub", login.UserId},
                {"nbf", notBefore},
                {"iat", issuedAt},
                {"exp", expiry}
            };

            var apikey = ConfigurationManager.AppSettings.Get("jwtSecretKey");

            var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

            return token;
        }

        private async Task<bool> AuthenticateUser(LoginRequest login)
        {
            using (var tccAuthentication = BusinessFacade.GetAuthenticationInstance())
            {
                var result = await tccAuthentication.ValidateUser(login);

                return (result.Result == "Validated");
            }
        }

        [Route("CreateTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateInAppPurchaseTransaction([FromBody] CreateTransactionRequest request)
        {
            ApiResponse<TransactionResponse> response = new ApiResponse<TransactionResponse>();
            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                var responseData = await marketPlace.CreateInAppPurchaseTransaction(request);
                response.Data = responseData;
                response.Status = "SUCCESS";
            }

            return Ok(response);
        }

        [Route("UpdateTransactionStatus")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateInAppPurchaseTransaction([FromBody] UpdateTransactionRequest request)
        {
            ApiResponse<UpdateTransactionResponse> response = new ApiResponse<UpdateTransactionResponse>();
            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                var responseData = await marketPlace.UpdateInAppPurchaseTransaction(request);
                response.Data = responseData;
                response.Status = "SUCCESS";
            }

            return Ok(response);
        }

        [Route("GetTransactionDetails")]
        [HttpPost]
        public async Task<IHttpActionResult> GetTransactionDetails([FromBody] CreateTransactionRequest request)
        {
            ApiResponse<TransactionDetailsResponse> response = new ApiResponse<TransactionDetailsResponse>();
            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                var responseData = await marketPlace.GetTransactionDetails(request);
                response.Data = responseData;
                response.Status = "SUCCESS";
            }
            return Ok(response);
        }
    }
}