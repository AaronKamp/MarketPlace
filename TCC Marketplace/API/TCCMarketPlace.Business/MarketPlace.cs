using System.Collections.Generic;
using TCCMarketPlace.Business.Interface;
using TCCMarketPlace.Model;
using TCCMarketPlace.Cache;
using System.Configuration;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using TCCMarketPlace.Model.ExceptionHandlers;
using System.Linq;
using TCCMarketPlace.Model.Logger;

namespace TCCMarketPlace.Business
{
    /// <summary>
    /// handles all business logic for Marketplace application
    /// </summary>
    public class MarketPlace : IMarketPlace
    {

        private string containerName = string.Empty;

        internal string ServiceUrl => ConfigurationManager.AppSettings["YLM.ApiUrl"];

        internal string APIKey => ConfigurationManager.AppSettings["YLM.Api_Key"];


        #region public functions
        /// <summary>
        /// Gets the list of available services and return it to the API controller. 
        /// </summary>
        /// <param name="currentUser" cref="User"> User information.</param>
        /// <param name="typeId"> Service type information </param>
        /// <param name="key"> Search key {optional} </param>
        /// <returns> Service List</returns> 
        public async Task<MarketPlaceServiceList> GetMarketPlaceList(User currentUser, int typeId, string key = null)
        {
            var list = await GetServiceList(currentUser, key, typeId, "All");

            return new MarketPlaceServiceList()
            {
                Services = list
            };
        }

        /// <summary>
        /// Gets the list of newly added services and return it to the API controller.
        /// </summary>
        /// <param name="currentUser" cref="User"> User information. </param>
        /// <param name="typeId"> Service type information. </param>
        /// <param name="key"> Search Key {optional} </param>
        /// <returns> Latest service list. </returns>
        public async Task<MarketPlaceServiceList> GetNewlyAddedServices(User currentUser, int typeId, string key = null)
        {
            var details = await GetServiceList(currentUser, key, typeId, "Newly Added");

            return new MarketPlaceServiceList()
            {
                Services = details
            };
        }

        /// <summary>
        /// Gets the list of subscribed services and return it to the API controller.
        /// </summary>
        /// <param name="currentUser" cref="User"> User information.</param>
        /// <param name="typeId"> Service type information. </param>
        /// <param name="key"> Search key {optional}.</param>
        /// <returns> Subscribed service list.</returns>
        public async Task<MarketPlaceServiceList> GetMyServices(User currentUser, int typeId, string key = null)
        {
            var list = await GetServiceList(currentUser, key, typeId, typeId == 1 ? "My Addons" : "My Offers");

            return new MarketPlaceServiceList()
            {
                Services = list
            };
        }
        /// <summary>
        /// Gets the details of the service selected by the user and returns it to the API controller.
        /// </summary>
        /// <param name="currentUser"> User information.</param>
        /// <param name="serviceId"> Id of the selected service. </param>
        /// <returns> ServiceDetails </returns>
        public async Task<Service> GetDetails(User currentUser, int serviceId)
        {
            // add code to get the service type

            return await GetServiceDetails(currentUser, serviceId);
        }

        /// <summary>
        /// Gets the available categories and returns it to the API controller.
        /// </summary>
        /// <param name="userName"> User Name</param>
        /// <param name="typeId"> Service type information. </param>
        /// <returns> List of categories.</returns>
        public List<Category> GetCategories(string userName, int typeId)
        {
            var categories = new List<Category>();
            //todo: TO implement in second phase once the categories are identified at marketplace admin.
          
            return categories;
        }

        /// <summary>
        /// Gets the list of slider  image URLs and return it to the API controller. 
        /// </summary>
        /// <param name="currentUser">User information.</param>
        /// <param name="typeId"> Service type information.</param>
        /// <returns> List of  Image URLs. </returns>
        public async Task<object> GetSlideShowImages(User currentUser, int typeId)
        {
            return await GetSlideImages(currentUser, typeId);
        }

        /// <summary>
        /// Toggle the enabled status of the selected service.
        /// </summary>
        /// <param name="currentUser" cref = "User"> User information.</param>
        /// <param name="service" cref="Service"> Service information.  </param>
        /// <returns> Updated service details. </returns>
        public async Task<Service> EnableOrDisableService(User currentUser, Service service)
        {
            var param = JsonConvert.SerializeObject(new
            {
                userid = currentUser.UserId.ToString(),
                thermostat_id = service.ThermostatId,
                ServiceId = service.ServiceId,
                Enable = service.IsEnabled ? "N" : "Y"
            });
            var result = await ThirdPartyAPIImplementation.PostValues(GetUrlForUpdateServiceSettings(), param);
            service.IsEnabled = !(service.IsEnabled);
            return service;
        }

        /// <summary>
        /// Unsubscribes a service for the user. Also un-enroll from service provider's database.
        /// </summary>
        /// <param name="currentUser" cref="User"> User information.</param>
        /// <param name="service" cref="Service"> Service information. </param>
        /// <returns> Success or failure status. </returns>
        public async Task<string> RemoveService(User currentUser, Service service)
        {
           
            if (!string.IsNullOrWhiteSpace(service.ServiceProviderId))
            {
                int serviceProviderId;
                ServiceProvider serviceProvider;

                //gets the service provider details if Service provider id is available in service details.
                if (int.TryParse(service.ServiceProviderId, out serviceProviderId))
                {
                    serviceProvider = GetServiceProviderDetails(serviceProviderId);
                    using (var thirdPartyService = BusinessFacade.GetServiceProviderInstance(serviceProvider))
                    {
                        //delist from third party enrolment.
                        await thirdPartyService.UnEnroll(currentUser, service);
                    }
                }
                else
                {
                    var ex = new BusinessException($"ServiceProviderId did not return valid service provider Id for {service.ServiceId},{currentUser.UserId}.");
                    new Log4NetLogger().Log(ex.Message, ex, LogLevelEnum.Information);
                }

            }

            var param = JsonConvert.SerializeObject(new
            {
                userid = currentUser.UserId.ToString(),
                thermostat_id = service.ThermostatId,
                ServiceId = service.ServiceId,
                Remove = "Y"
            });

            var result = await ThirdPartyAPIImplementation.PostValues(GetUrlForUpdateServiceSettings(), param);
            return result;
        }

        /// <summary>
        /// Gets the available service provider details, cache it and return it to the caller.
        /// </summary>
        /// <param name="serviceProviderId">Service Provider id as in service details.</param>
        /// <returns> Service provider details. </returns>
        private ServiceProvider GetServiceProviderDetails(int serviceProviderId)
        {
            var list = new List<ServiceProvider>();
            //caches the service provider collection.
            try
            {
                list = CacheManager.Instance.GetItem<List<ServiceProvider>>("ServiceProviderCollection");
                if ( list == null || ! list.Any())
                {
                    list = ServiceProviderManager.GetServiceProviderList().ToList();
                    CacheManager.Instance.PutItem<List<ServiceProvider>>("ServiceProviderCollection", list, new TimeSpan(0, 5, 0));
                }
            }
            catch(RedisCacheException ex)
            {
                if (!list.Any())
                {
                    list = ServiceProviderManager.GetServiceProviderList().ToList();
                }
                //Redis cache exception detected and logged as warning. 

                var exceptionIdentifier = Guid.NewGuid();
                new Log4NetLogger().Log(LogHelper.ComposeExceptionLog(ex.ExceptionMessage,exceptionIdentifier), ex.RedisException, LogLevelEnum.Warning);
            }
            catch
            {
                throw;
            }
            return list.Where(p => p.Id == serviceProviderId).FirstOrDefault();
        }

        /// <summary>
        /// Saves the report URL generated on  enrolment.
        /// </summary>
        /// <param name="currentUser" cref="User"> User information.</param>
        /// <param name="service" cref="Service"> Service information.</param>
        /// <returns> Success or failure status. </returns>
        public async Task<string> SaveReportUrl(User currentUser, Service service)
        {
            var param = JsonConvert.SerializeObject(new
            {
                userid = currentUser.UserId.ToString(),
                thermostat_id = service.ThermostatId,
                ServiceId = service.ServiceId,
                Reporturl = HttpUtility.UrlDecode(service.ReportUrl)
            });

            var result = await ThirdPartyAPIImplementation.PostValues(GetUrlForUpdateServiceSettings(), param);
            return result;
        }

        /// <summary>
        /// Subscribes a service for the user in case of free service.
        /// </summary>
        /// <param name="currentUser" cref="User"> User information.</param>
        /// <param name="service" cref="Service"> Service information. </param>
        /// <returns> Success or failure information. </returns>
        public async Task<string> SubscribeToService(User currentUser, Service service)
        {
            // gets service details.
            var serviceDetails = await GetServiceDetails(currentUser, service.ServiceId);

            // checks if already already subscribed.
            if (serviceDetails.IsBought)
            {
                return "Service already subscribed";
            }

            else
            {
                var param = JsonConvert.SerializeObject(new
                {
                    userid = currentUser.UserId,
                    thermostat_id = service.ThermostatId,
                    ServiceId = service.ServiceId,
                    purchaseamount = service.Price
                });
                var result = await ThirdPartyAPIImplementation.PostValues(GetUrlForSubscribeToService(), param);
                return result;
            }
        }

        /// <summary>
        /// Subscribes a service for the user in case of paid service.
        /// </summary>
        /// <param name="currentUser" cref="User"> User information.</param>
        /// <param name="service" cref="Service"> Service information. </param>
        /// <returns> Success or failure information. </returns>
        public async Task<string> SubscribeToService(UpdateTransactionRequest request)
        {
            var param = JsonConvert.SerializeObject(new
            {
                userid = request.UserId,
                thermostat_id = request.ThermostatId,
                ServiceId = request.ServiceId,
                purchaseamount = request.PurchaseAmount,
                InAppTransactionId = request.InAppTransactionId,
                Purchase_type = request.PurchaseType

            });
            var result = await ThirdPartyAPIImplementation.PostValues(GetUrlForSubscribeToService(), param);
            return result;
        }

        /// <summary>
        /// Initiates a transaction for a paid service. 
        /// </summary>
        /// <param name="request" cref="CreateTransactionRequest"></param>
        /// <returns> Transaction id of the initiated transaction.</returns>
        public async Task<TransactionResponse> CreateInAppPurchaseTransaction(CreateTransactionRequest request)
        {
            var param = JsonConvert.SerializeObject(new
            {
                userid = request.UserId,
                thermostat_id = request.ThermostatId,
                ServiceId = request.ServiceId,
                statusid = (int)TransactionStatus.Initialized,
                inapppurchaseid = request.ProductId,
                Devicetype = request.DeviceType
            });
            // creates new transaction with initialized status.
            var result = await ThirdPartyAPIImplementation.PostValues(GetUrlForCreateTransaction(), param);
            var response = JsonConvert.DeserializeObject<CreateTransactionResponse>(result);

            return new TransactionResponse()
            {
                TransactionId = Convert.ToInt32(response.Message.Split('=')[1])
            };
        }

        /// <summary>
        /// Update the transaction status to success or failure.
        /// </summary>
        /// <param name="request" cref="UpdateTransactionRequest"></param>
        /// <returns> Transaction Id and Marketplace details page URL.</returns>
        public async Task<TransactionResponse> UpdateInAppPurchaseTransaction(UpdateTransactionRequest request)
        {
            string subscribeResult = string.Empty;

            var user = new User { UserId = request.UserId, ThermostatId = request.ThermostatId };

            //Get the service details from database
            var serviceDetails = await GetServiceDetails(user, request.ServiceId);

            if (request.PurchaseStatus && serviceDetails.IsBought == false)
            {
                subscribeResult = await SubscribeToService(request);
            }

            //gets the transaction details.
            var transaction = await GetTransaction(request.TransactionId);

            var transactionStatus = Convert.ToInt32(transaction.Status);

            if (transactionStatus == (int)TransactionStatus.Initialized)
            {
                var param = JsonConvert.SerializeObject(new
                {
                    transactionid = request.TransactionId,
                    statusid = request.PurchaseStatus ? (int)TransactionStatus.Success : (int)TransactionStatus.Failed
                });

                //updates transaction status.
                var result = await ThirdPartyAPIImplementation.PostValues(GetUrlForUpdateTransaction(), param);
            }
            // gets marketplace details page URL.
            string detailsUrl = GetDetailsUrl(request.ServiceId, serviceDetails.ServiceTypeId);

            return new TransactionResponse()
            {
                TransactionId = request.TransactionId,
                DetailsUrl = detailsUrl
            };
        }

        /// <summary>
        /// Generates the the marketplace details URL for the selected service.
        /// </summary>
        /// <param name="serviceId"> Service Id.</param>
        /// <param name="serviceTypeId"> Service type Id.</param>
        /// <returns> Marketplace Details URL. </returns>
        public string GetDetailsUrl(int serviceId, int serviceTypeId)
        {
            string hostedDetailsurl = ConfigurationManager.AppSettings["MarketPlaceDetailsUrl"];

            if (hostedDetailsurl.Contains("https") == false)
            {
                hostedDetailsurl = hostedDetailsurl.Replace("http", "https");
            }

            string detailsUrl = hostedDetailsurl + serviceId + "/" + serviceTypeId + "/" + "All/";
            return detailsUrl;
        }
        
        /// <summary>
        /// Gets the latest transaction details for a user service combination and returns it to the API controller.
        /// </summary>
        /// <param name="request" cref="CreateTransactionRequest"></param>
        /// <returns> latest transaction details.</returns>
        public async Task<TransactionDetailsResponse> GetTransactionDetails(CreateTransactionRequest request)
        {
            var result = await ThirdPartyAPIImplementation.GetValues(GetUrlForTransactionDetails(request.UserId, request.ProductId, request.DeviceType));

            IEnumerable<LatestTransactionDetails> transaction = JsonConvert.DeserializeObject<IEnumerable<LatestTransactionDetails>>(result);

            if (transaction.Any())
            {
                var tr = transaction.OrderByDescending(t => t.Transaction_Id).FirstOrDefault();

                return new TransactionDetailsResponse
                {
                    ServiceId = tr.Service_Id,
                    UserId = tr.User_Id,
                    ThermostatId = tr.Thermostat_Id,
                    TransactionId = tr.Transaction_Id,
                    Status = tr.Status_Id.ToString()
                };
            }

            return new TransactionDetailsResponse();
        }

        /// <summary>
        /// Checks if the selected service is already subscribed.
        /// </summary>
        /// <param name="currentUser" cref="User"> User information. </param>
        /// <param name="serviceId"> Service information. </param>
        /// <returns> True if subscribed. Else false</returns>
        public async Task<bool> IsServiceSubscribed(User currentUser, int serviceId)
        {
            var result = await ThirdPartyAPIImplementation.GetValues(GetUrlForServiceDetails(currentUser, serviceId));
            YLMService details = JsonConvert.DeserializeObject<YLMService>(result);
            bool isBought = details.Signup_Status == "Y" ? true : false;
            return isBought;
        }

        #endregion

        #region Private Method implementations
        /// <summary>
        /// Gets the service list as required for the public methods GetMarketPlaceList, GetNewlyAddedServices, GetMyServices
        /// </summary>
        private async Task<List<Service>> GetServiceList(User currentUser, string keyword, int typeId, string categoryName)
        {
            var services = new List<Service>();

            var result = await ThirdPartyAPIImplementation.GetValues(GetUrlForServiceList(currentUser, keyword, typeId, categoryName));
            IEnumerable<YLMService> yLMServices = JsonConvert.DeserializeObject<IEnumerable<YLMService>>(result);
            services = yLMServices.Select(x => new Service()
            {
                Title = x.Service_Title,
                ShortDescription = x.Service_Short_Desc,
                ServiceId = x.Service_Id,
                ServiceTypeId = typeId,
                ImageUrl = x.Icon_Image,
                // casting y to true and n to false
                IsBought = x.Signup_Status == "Y" ? true : false,
            }).ToList();

            return services;
        }
        /// <summary>
        /// Gets the list of slider image URL for the public function GetSlideShowImages.
        /// </summary>
        private async Task<object> GetSlideImages(User currentUser, int typeId)
        {
            var result = await ThirdPartyAPIImplementation.GetValues(GetUrlForSlideImages(currentUser, typeId));

            var imageList = JsonConvert.DeserializeObject<IEnumerable<SlideShowImage>>(result);

            return imageList.Select(img => new
            {
                ServiceId = img.ServiceId,
                ImageBlobUrl = img.ImageBlobUrl
            }).ToList();
        }
        /// <summary>
        /// Gets the service details for the public methods GetDetails and SubscribetoService. 
        /// </summary>
        private async Task<Service> GetServiceDetails(User currentUser, int serviceId)
        {
            var result = await ThirdPartyAPIImplementation.GetValues(GetUrlForServiceDetails(currentUser, serviceId));
            YLMService details = JsonConvert.DeserializeObject<YLMService>(result);
            var service = new Service()
            {
                Title = details.Service_Title,
                ShortDescription = details.Service_Short_Desc,
                LongDescription = details.Service_Long_Desc,
                ServiceId = serviceId,
                ImageUrl = details.Icon_Image,
                ServiceTypeId = details.Service_Type_Id,
                IsBought = details.Signup_Status == "Y" ? true : false,
                ReportUrl = HttpUtility.UrlEncode(details.Report_Url),

                //Adding service and user information for service provider enrolment.
                SignUpUrl = HttpUtility.UrlEncode(string.Format("{0}?progid={1}&partneruid={2}&partnerdevid={3}&serviceID={4}&thermostatId={5}&locationId={6}",
                            details.Url, details.Partner_Promo_Code, currentUser.UserId, currentUser.MacID, details.Service_Id, currentUser.ThermostatId, currentUser.LocationId)),

                IsEnabled = details.Serv_Enable == "Y" ? true : false,
                Price = details.Purchase_Price,
                ProductId = details.Inapp_Purchase_Id,
                PartnerPromoCode = details.Partner_Promo_Code,
                ThermostatId = currentUser.ThermostatId,
                ServiceProviderId = details.Service_Provider
            };
            return service;
        }
        /// <summary>
        /// Gets the transaction details for the public function UpdateInAppPurchaseTransaction.
        /// </summary>
        private async Task<TransactionDetailsResponse> GetTransaction(int transactionId)
        {
            var result = await ThirdPartyAPIImplementation.GetValues(GetUrlForReadTransacion(transactionId));

            TransactionDetails transaction = JsonConvert.DeserializeObject<TransactionDetails>(result);

            return new TransactionDetailsResponse
            {
                ServiceId = transaction.Service_Id,
                UserId = transaction.User_Id,
                ThermostatId = transaction.Thermostat_Id,
                TransactionId = transaction.Transaction_Id,
                Status = transaction.Status_Id.ToString()
            };
        }

        #endregion

        #region Private Methods : Generate API URL Methods
        /// <summary>
        /// Generate YLM's API URL for service list
        /// </summary>
        private string GetUrlForServiceList(User user, string keyword, int typeId, string categoryName)
        {
            StringBuilder strQueryParams = new StringBuilder();
            if (categoryName == "Newly Added")
            {
                strQueryParams.Append(ServiceUrl + "service/newest?" + string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey) + "&"));
            }
            else if (categoryName == "My Addons" || categoryName == "My Offers")
            {
                strQueryParams.Append(ServiceUrl + "service/subscribed?" + string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey) + "&"));
            }
            else
            {
                strQueryParams.Append(ServiceUrl + "service?" + string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey) + "&"));
            }
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("userid"), HttpUtility.UrlEncode(user.UserId.ToString())) + "&");
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("userId_type"), HttpUtility.UrlEncode(user.UserType)) + "&");
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("location_id"), HttpUtility.UrlEncode(user.LocationId.ToString())) + "&");
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("thermostat_id"), HttpUtility.UrlEncode(user.ThermostatId.ToString())) + "&");
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("service_type"), HttpUtility.UrlEncode(typeId == 1 ? "1" : "2")));
            if ((!string.IsNullOrEmpty(user.ZipCode)) && (categoryName != "My Addons" && categoryName != "My Offers"))
            {
                strQueryParams.Append("&" + string.Format("{0}={1}", HttpUtility.UrlEncode("zip_code"), HttpUtility.UrlEncode(user.ZipCode)));
            }
            if (categoryName == "All")
            {
                strQueryParams.Append("&" + string.Format("{0}={1}", HttpUtility.UrlEncode("search_string"), HttpUtility.UrlEncode(keyword)));
            }

            return strQueryParams.ToString();
        }
        /// <summary>
        /// Generate YLM's API URL for slider images.
        /// </summary>
        private string GetUrlForSlideImages(User user, int typeId)
        {
            StringBuilder strQueryParams = new StringBuilder();
            strQueryParams.Append(ServiceUrl + "service/slideshow?");
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("userid"), HttpUtility.UrlEncode(user.UserId.ToString()) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("service_type"), HttpUtility.UrlEncode(typeId.ToString()) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("thermostat_id"), HttpUtility.UrlEncode(user.ThermostatId.ToString()) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("location_id"), HttpUtility.UrlEncode(user.LocationId.ToString()) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("userid_type"), HttpUtility.UrlEncode(user.UserType)));
            if (!string.IsNullOrEmpty(user.ZipCode))
            {
                strQueryParams.Append("&" + string.Format("{0}={1}", HttpUtility.UrlEncode("zip_code"), HttpUtility.UrlEncode(user.ZipCode)));
            }
            return strQueryParams.ToString();
        }
        /// <summary>
        /// Generate YLM's API URL for service details.
        /// </summary>
        private string GetUrlForServiceDetails(User user, int serviceId)
        {
            StringBuilder strQueryParams = new StringBuilder();
            strQueryParams.Append(ServiceUrl + "service/");
            strQueryParams.Append(HttpUtility.UrlEncode(user.UserId.ToString()) + ",");
            strQueryParams.Append(HttpUtility.UrlEncode(serviceId.ToString()) + ",");
            strQueryParams.Append(HttpUtility.UrlEncode(user.ThermostatId.ToString()) + "?");
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey)));

            return strQueryParams.ToString();
        }

        /// <summary>
        /// Generate YLM's API URL for updating service for a user.
        /// Remove service, Save report URL, Enable  or disable service uses this API.
        /// </summary>
        private string GetUrlForUpdateServiceSettings()
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.Append(ServiceUrl + "userService/update?");
            strQuery.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey)));
            return strQuery.ToString();
        }
        /// <summary>
        /// Generate YLM's API URL for Service subscription.
        /// </summary>
        private string GetUrlForSubscribeToService()
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.Append(ServiceUrl + "userService/subscribe?");
            strQuery.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey)));
            return strQuery.ToString();
        }
        /// <summary>
        /// Generate YLM's API URL for creating Transaction
        /// </summary>
        private string GetUrlForCreateTransaction()
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.Append(ServiceUrl + "transaction/create?");
            strQuery.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey)));
            return strQuery.ToString();
        }
        /// <summary>
        /// Generate YLM's API URL for updating transaction details.
        /// </summary>
        private string GetUrlForUpdateTransaction()
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.Append(ServiceUrl + "transaction/update?");
            strQuery.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey)));
            return strQuery.ToString();
        }
        /// <summary>
        /// Generate YLM's API URL for latest transaction details.
        /// </summary>
        private string GetUrlForTransactionDetails(int userId, string inAppPurchaseId, int deviceType)
        {
            StringBuilder strQueryParams = new StringBuilder();
            strQueryParams.Append(ServiceUrl + "transaction/latest?");
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("userid"), HttpUtility.UrlEncode(userId.ToString()) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("inapppurchaseid"), HttpUtility.UrlEncode(inAppPurchaseId) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("Devicetype"), HttpUtility.UrlEncode(deviceType.ToString())));
            return strQueryParams.ToString();

        }
        /// <summary>
        /// Generate YLM's API URL for transaction details by transaction Id.
        /// </summary>
        private string GetUrlForReadTransacion(int transactionId)
        {
            StringBuilder strQueryParams = new StringBuilder();
            strQueryParams.Append(ServiceUrl + "transaction/");
            strQueryParams.Append(HttpUtility.UrlEncode(transactionId.ToString()) + "?");
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey)));
            return strQueryParams.ToString();
        }

        #endregion
        public virtual void Dispose()
        {
        }


    }
}
