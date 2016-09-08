using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TCCMarketPlace.Apis.Models;
using TCCMarketPlace.Business;
using TCCMarketPlace.Model;

namespace TCCMarketPlace.Apis.Controllers
{
    /// <summary>
    /// All web request from the TCC MarketPlace mobile application is served through MarketPlaceController.
    /// All request contains JWT token in the headers which is resolved by the AuthHandler for authentication. 
    /// </summary>
    [Authorize]
    [RoutePrefix("api/MarketPlace")]
    public class MarketPlaceController : BaseController
    {
        /// <summary>
        /// Returns list of all available services.
        /// </summary>
        /// <param name="typeId">Used to indicate if Add-ons or Offers need to be returned.</param>
        /// <returns> List of available Add-ons or offers according to typeId</returns>
        [Route("GetMarketPlaceList/{typeId}")]
        public async Task<ApiResponse<MarketPlaceServiceList>> GetMarketPlaceList(int typeId)
        {
            ApiResponse<MarketPlaceServiceList> response = new ApiResponse<MarketPlaceServiceList>();

            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                var result = await marketPlace.GetMarketPlaceList(CurrentUser, typeId);
                response.Data = result;
            }
            return response;
        }
        /// <summary>
        /// Returns the list of the latest services available.
        /// </summary>
        /// <param name="typeId"> Used to indicate if Add-ons or Offers need to be returned.</param>
        /// <returns>List of newest Add-ons or offers according to typeId.</returns>
        [Route("GetNewlyAddedServices/{typeId}")]
        public async Task<ApiResponse<MarketPlaceServiceList>> GetNewlyAddedServices(int typeId)
        {
            ApiResponse<MarketPlaceServiceList> response = new ApiResponse<MarketPlaceServiceList>();

            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketPlace.GetNewlyAddedServices(CurrentUser, typeId);
            }

            return response;
        }
        /// <summary>
        /// Returns the list of all services subscribed by the user.
        /// </summary>
        /// <param name="typeId">Used to indicate if Add-ons or Offers need to be returned.</param>
        /// <returns> List of subscribed services according to typeId.</returns>
        [Route("GetMyServices/{typeId}")]
        public async Task<ApiResponse<MarketPlaceServiceList>> GetMyServices(int typeId)
        {
            ApiResponse<MarketPlaceServiceList> response = new ApiResponse<MarketPlaceServiceList>();

            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketPlace.GetMyServices(CurrentUser, typeId);
            }

            return response;
        }
        /// <summary>
        /// Returns the list of available categories for the selected service type. 
        /// </summary>
        /// <param name="typeId"> Used to indicate if categories of Add-ons or Offers need to be returned. </param>
        /// <returns> List of available categories.</returns>
        [Route("GetCategories/{typeId}")]
        public ApiResponse<List<Category>> GetCategories(int typeId)
        {
            // To be implemented in second phase with dynamic category options.

            ApiResponse<List<Category>> response = new ApiResponse<List<Category>>();

            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = marketPlace.GetCategories(string.Empty, typeId);
            }

            return response;
        }

        /// <summary>
        /// Serves the list of URLs for slider images under the selected service type.
        /// </summary>
        /// <param name="typeId"> Used to indicate service type to filter SlideShowImage URLs.</param>
        /// <returns>List of Slider Image URLs.</returns>
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

        /// <summary>
        /// Serves the details of selected service.
        /// </summary>
        /// <param name="serviceId"> Used to identify the selected service. </param>
        /// <returns> Details of the selected service. </returns>
        [Route("GetDetails/{serviceId}")]
        public async Task<ApiResponse<Service>> GetDetails(int serviceId)
        {
            ApiResponse<Service> response = new ApiResponse<Service>();

            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketPlace.GetDetails(CurrentUser, serviceId);
            }

            return response;
        }
        /// <summary>
        /// Checks if the requested service is already subscribed.
        /// </summary>
        /// <param name="serviceId"> Used to identify the selected service. </param>
        /// <returns> True if service is already subscribed. Otherwise false.  </returns>

        [Route("IsSubscribed/{serviceId}")]
        public async Task<ApiResponse<bool>> GetServiceSubscriptionStatus(int serviceId)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                response.Data = await marketPlace.IsServiceSubscribed(CurrentUser, serviceId);
            }
            return response;
        }
        /// <summary>
        /// Toggles the service enabled status for each user.
        /// </summary>
        /// <param name="service" cref="Service"></param>
        /// <returns>Updated Service details</returns>
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
        /// <summary>
        /// Unsubscribes a service for the user.
        /// </summary>
        /// <param name="service" cref="Service"></param>
        /// <returns>Success information as string.</returns>
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
        /// <summary>
        /// Saves the report URL generated by service provider on enrolment.
        /// </summary>
        /// <param name="service" cref="Service"></param>
        /// <returns> Success or failure information as string. </returns>
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
        /// <summary>
        /// Activates the requested service for the user.
        /// </summary>
        /// <param name="service" cref="Service"></param>
        /// <returns> Success or failure information as string. </returns>
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
        /// <summary>
        ///  Authenticates user details sent from TCC native app.
        /// </summary>
        /// <param name="login" cref="LoginRequest"></param>
        /// <returns>Marketplace application landing page URL with the auth token. </returns>
        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        [System.Web.Mvc.RequireHttps]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequest login)
        {
            // add code for user authentication
            ApiResponse<LoginResponse> response = new ApiResponse<LoginResponse>();

            //Authenticate login credentials against TCC server
            var isAuthenticated = await AuthenticateUser(login);

            if (isAuthenticated)
            {
                string hostedurl = ConfigurationManager.AppSettings["MarketPlaceUrl"];

                if (hostedurl.Contains("https") == false)
                {
                    hostedurl = hostedurl.Replace("http", "https");
                }

                //Generate new JWT token for the logged in user with claims
                var token = new JwToken.TokenGenerator().GenerateToken(login);

                var apiBaseUri = HttpUtility.UrlEncode(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api");

                if (apiBaseUri.Contains("https") == false)
                {
                    apiBaseUri = apiBaseUri.Replace("http", "https");
                }

                //landing page URL for native app to navigate to on Web-View
                var landingPageUrl = $"{hostedurl}#/UserAuth/{token}/{apiBaseUri}";

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

        /// <summary>
        /// Authenticate login credentials against TCC server
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private async Task<bool> AuthenticateUser(LoginRequest login)
        {
            using (var tccAuthentication = BusinessFacade.GetAuthenticationInstance())
            {
                var result = await tccAuthentication.ValidateUser(login);

                return (result.Result == "Validated");
            }
        }

        /// <summary>
        /// Creates a transaction to represent a purchase initiated in the Marketplace application.
        /// </summary>
        /// <param name="request" cref="CreateTransactionRequest"></param>
        /// <returns>Transaction Id and the market place details page URL.</returns>
        [Route("CreateTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateInAppPurchaseTransaction([FromBody] CreateTransactionRequest request)
        {
            ApiResponse<TransactionResponse> response = new ApiResponse<TransactionResponse>();
            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                var user = new User
                {
                    UserId = request.UserId,
                    ThermostatId = request.ThermostatId
                };
                var serviceDetails = await marketPlace.GetDetails(user, request.ServiceId);

                if (serviceDetails.IsBought)
                {
                    var responseData = new TransactionResponse
                    {
                        TransactionId = -1,
                        DetailsUrl = marketPlace.GetDetailsUrl(request.ServiceId, serviceDetails.ServiceTypeId)
                    };
                    response.Data = responseData;
                    response.Status = "SUCCESS";
                }
                else
                {
                    var responseData = await marketPlace.CreateInAppPurchaseTransaction(request);
                    response.Data = responseData;
                    response.Status = "SUCCESS";
                }
            }

            return Ok(response);
        }
        /// <summary>
        /// Updates the status of transaction to success or failure and
        /// subscribes the service on transaction success.
        /// </summary>
        /// <param name="request" cref="UpdateTransactionRequest"></param>
        /// <returns></returns>
        [Route("UpdateTransactionStatus")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateInAppPurchaseTransaction([FromBody] UpdateTransactionRequest request)
        {

            ApiResponse<TransactionResponse> response = new ApiResponse<TransactionResponse>();
            using (var marketPlace = BusinessFacade.GetMarketPlaceInstance())
            {
                var responseData = await marketPlace.UpdateInAppPurchaseTransaction(request);
                response.Data = responseData;
                response.Status = "SUCCESS";
            }

            return Ok(response);
        }

        /// <summary>
        /// Gets the details of latest transaction initiated by the user.
        /// </summary>
        /// <param name="request" cref="CreateTransactionRequest"></param>
        /// <returns>Transaction details. </returns>
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