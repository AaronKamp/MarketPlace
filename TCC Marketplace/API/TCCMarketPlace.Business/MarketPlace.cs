using System.Collections.Generic;
using TCCMarketPlace.Business.Interface;
using TCCMarketPlace.Model;
using System.Configuration;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.Linq;
using System.Web.Security;
using System.Xml.Linq;
using System.IO;

namespace TCCMarketPlace.Business
{
    public class MarketPlace : IMarketPlace
    {
        private string containerName = string.Empty;

        internal string ServiceUrl => ConfigurationManager.AppSettings["YLM_ServiceUrl"];

        internal string APIKey => ConfigurationManager.AppSettings["YLM_APIKey"];

        internal string EarthApiUrl => ConfigurationManager.AppSettings["EarthNetworkServiceUrl"];

        internal string PartnerName => ConfigurationManager.AppSettings["EarthNetworkPartnerName"];

        #region public functions

        public async Task<MarketPlaceDetails> GetMarketPlaceList(User currentUser, int typeId, string key)
        {
            var list = await GetServiceList(currentUser, key, typeId, "All");

            return new MarketPlaceDetails()
            {
                Services = list
            };
        }

        public async Task<MarketPlaceDetails> GetNewlyAddedServices(User currentUser, int typeId, string key)
        {
            var details = await GetServiceList(currentUser, key, typeId, "Newly Added");

            return new MarketPlaceDetails()
            {
                Services = details
            };
        }

        public async Task<MarketPlaceDetails> GetMyServices(User currentUser, int typeId, string key)
        {
            var list = await GetServiceList(currentUser, key, typeId, typeId == 1 ? "My Addons" : "My Offers");

            return new MarketPlaceDetails()
            {
                Services = list
            };
        }

        public async Task<Service> GetDetails(User currentUser, int serviceId)
        {
            // add code to get the service type

            return await GetServiceDetails(currentUser, serviceId);
        }

        public List<Category> GetCategories(string userName, int typeId)
        {
            var categories = new List<Category>();
            //sample implementation, need to be changed
            for (int i = 4; i <= 10; i++)
            {
                categories.Add(new Category()
                {
                    Id = i,
                    Name = "Category " + i
                });
            }

            return categories;
        }

        public async Task<object> GetSlideShowImages(User currentUser, int typeId)
        {
            return await GetSlideImages(currentUser, typeId);
        }

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

        public async Task<string> RemoveService(User currentUser, Service service)
        {
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

        public async Task<bool> UnEnrollFromEarthNetwork(int userId)
        {
            var unEnroll = await ThirdPartyAPIImplementation.DeleteValues(GetUrlForUnEnrollEarthNetworkService(userId).ToString());

            var unEnrollResult = JsonConvert.DeserializeObject<DeleteResponse>(unEnroll);

            if (string.Equals(unEnrollResult.Status, "Cancelled"))
                return true;
            else return false;
        }

        public async Task<string> SubscribeToService(User currentUser, Service service)
        {

            var serviceDetails = await GetServiceDetails(currentUser, service.ServiceId);

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

        private async Task<Service> UpdateReportUrl(User currentUser, Service service)
        {
            var param = JsonConvert.SerializeObject(new
            {
                userid = currentUser.UserId,
                thermostat_id = service.ThermostatId,
                ServiceId = service.ServiceId,
                Reporturl = ""
            });
            var result = await ThirdPartyAPIImplementation.PostValues(GetUrlForUpdateServiceSettings(), param);
            return service;
        }

        public async Task<TransactionResponse> CreateInAppPurchaseTransaction(CreateTransactionRequest request)
        {
            var param = JsonConvert.SerializeObject(new
            {
                userid = request.UserId,
                thermostat_id = request.ThermostatId,
                ServiceId = request.ServiceId,
                statusid = (int)TransactionStatus.Initialised,
                inapppurchaseid = request.ProductId,
                Devicetype = request.DeviceType
            });

            var result = await ThirdPartyAPIImplementation.PostValues(GetUrlForCreateTransaction(), param);
            var repsonse = JsonConvert.DeserializeObject<CreateTransactionResponse>(result);

            return new TransactionResponse()
            {
                TransactionId = Convert.ToInt32(repsonse.Message.Split('=')[1])
            };
        }

        public async Task<UpdateTransactionResponse> UpdateInAppPurchaseTransaction(UpdateTransactionRequest request)
        {
            string subscribeResult = string.Empty;

            var user = new User { UserId = request.UserId, ThermostatId = request.ThermostatId };

            var serviceDetails = await GetServiceDetails(user, request.ServiceId);

            if (request.PurchaseStatus && serviceDetails.IsBought == false)
            {
                subscribeResult = await SubscribeToService(request);
            }

            var transaction = await GetTransaction(request.TransactionId);

            var transactionStatus = Convert.ToInt32(transaction.Status);

            if (transactionStatus == (int)TransactionStatus.Initialised)
            {
                var param = JsonConvert.SerializeObject(new
                {
                    transactionid = request.TransactionId,
                    statusid = request.PurchaseStatus ? (int)TransactionStatus.Success : (int)TransactionStatus.Failed
                });

                var result = await ThirdPartyAPIImplementation.PostValues(GetUrlForUpdateTransaction(), param);
            }

            string hostedDetailsurl = ConfigurationManager.AppSettings["MarketPlaceDetailsUrl"];

            string detailsUrl = hostedDetailsurl + request.ServiceId + "/" + serviceDetails.ServiceTypeId + "/" + "All/";

            return new UpdateTransactionResponse()
            {
                TransactionId = request.TransactionId,
                DetailsUrl = detailsUrl
            };
        }
        
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

        #endregion

        #region Private Method implementations
        private async Task<List<Service>> GetServiceList(User currentUser, string keyword, int typeId, string categoryName)
        {
            var services = new List<Service>();

            var result = await ThirdPartyAPIImplementation.GetValues(GetUrlForServiceList(currentUser, keyword, typeId, categoryName));
            IEnumerable<YLMService> yLMServices = JsonConvert.DeserializeObject<IEnumerable<YLMService>>(result);
            services = yLMServices.Select(x => new Service()
            {
                Title = x.Service_Title,
                Description = x.Service_Short_Desc,
                ServiceId = x.Service_Id,
                ServiceTypeId = typeId,
                ImageUrl = x.Icon_Image,
                IsBought = x.Signup_Status == "Y" ? true : false,
            }).ToList();

            return services;
        }

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

        private async Task<Service> GetServiceDetails(User currentUser, int serviceId)
        {
            var result = await ThirdPartyAPIImplementation.GetValues(GetUrlForServiceDetails(currentUser, serviceId));
            YLMService details = JsonConvert.DeserializeObject<YLMService>(result);
            return new Service()
            {
                Title = details.Service_Title,
                Description = details.Service_Short_Desc,
                ServiceId = serviceId,
                ImageUrl = details.Icon_Image,
                ServiceTypeId = details.Service_Type_Id,
                IsBought = details.Signup_Status == "Y" ? true : false,
                ReportUrl = details.Signup_Status == "Y" ? HttpUtility.UrlEncode(details.Report_Url) : "",
                SignUpUrl = HttpUtility.UrlEncode(string.Format("{0}?UserID={1}&UserName={2}&ServiceID={3}&ThermostatID={4}&LocationID={5}&MacID={6}&PartnerPromoCode={7}",
                            details.Url, currentUser.UserId, currentUser.UserName, details.Service_Id, currentUser.ThermostatId, currentUser.LocationId, currentUser.MacID, details.Partner_Promo_Code)),
                IsEnabled = details.Serv_Enable == "Y" ? true : false,
                Price = details.Purchase_Price,
                ProductId = details.Inapp_Purchase_Id,
                ThermostatId = currentUser.ThermostatId
            };
        }

        //private async Task<bool> IsServiceSubscribed(User currentUser, int serviceId)
        //{
        //    var typeId = 1;
        //    var service = await GetServiceDetails(currentUser, serviceId, typeId);
        //    return service.IsBought;
        //}

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

        #region Generate API url Methods
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

        private async Task<string> GetUrlForUnEnrollEarthNetworkService(int userId)
        {
            string accessToken = await GetToken("EarthNetwork");
            StringBuilder strQueryParams = new StringBuilder();
            strQueryParams.Append(EarthApiUrl + "weatherbughome-staging/api/enroll?");
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("partnerName"), HttpUtility.UrlEncode(PartnerName) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("customerIds"), HttpUtility.UrlEncode(userId.ToString()) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("access_token"), HttpUtility.UrlEncode(accessToken)));
            return strQueryParams.ToString();
        }

        private string GetUrlForAccessToken()
        {
            var consumerKey = ConfigurationManager.AppSettings["Consumer_Key"];
            var secretKey = ConfigurationManager.AppSettings["Secret_Key"];
            StringBuilder strQueryParams = new StringBuilder();
            strQueryParams.Append(EarthApiUrl + "oauth20/token?");
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("client_Id"), HttpUtility.UrlEncode(consumerKey) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("client_secret"), HttpUtility.UrlEncode(secretKey) + "&"));
            strQueryParams.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("grant_type"), HttpUtility.UrlEncode("client_credentials")));
            return strQueryParams.ToString();
        }

        private async Task<string> GetToken(string providerName)
        {
            XDocument document = new XDocument();

            if (!File.Exists(HttpContext.Current.Server.MapPath("~/XML/AccessToken.xml")))
            {
                document.Add(new XElement("TokenData"));
                document.Save(HttpContext.Current.Server.MapPath("~/XML/AccessToken.xml"));
            }

            document = XDocument.Load(HttpContext.Current.Server.MapPath("~/XML/AccessToken.xml"));
            XElement rootElement = document.Descendants("TokenData").FirstOrDefault();
            var token = from providers in rootElement.Descendants("ServiceProvider")
                                .Where(providers => string.Equals((string)providers.Attribute("Name"), providerName, StringComparison.OrdinalIgnoreCase))
                        select providers.Element("Token").Value;

            if (token != null && token.Any())
            {
                return token.FirstOrDefault();
            }
            else
            {
                var providerToken = await GetTokenFromAPI(providerName);

                rootElement.Add(new XElement("ServiceProvider",
                                            new XAttribute("Name", providerName),
                                            new XElement("Token", providerToken)));
                document.Save(HttpContext.Current.Server.MapPath("~/XML/AccessToken.xml"));
                return providerToken;
            }


        }

        private async Task<string> GetTokenFromAPI(string providerName)
        {
            var result = await ThirdPartyAPIImplementation.GetValues(GetUrlForAccessToken());
            var response = JsonConvert.DeserializeObject<EarthNetworkTokenResponse>(result);
            return response.OAuth.AccessToken.Token;
        }

        private string GetUrlForUpdateServiceSettings()
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.Append(ServiceUrl + "userService/update?");
            strQuery.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey)));
            return strQuery.ToString();
        }
        private string GetUrlForSubscribeToService()
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.Append(ServiceUrl + "userService/subscribe?");
            strQuery.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey)));
            return strQuery.ToString();
        }

        private string GetUrlForCreateTransaction()
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.Append(ServiceUrl + "transaction/create?");
            strQuery.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey)));
            return strQuery.ToString();
        }
        private string GetUrlForUpdateTransaction()
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.Append(ServiceUrl + "transaction/update?");
            strQuery.Append(string.Format("{0}={1}", HttpUtility.UrlEncode("apikey"), HttpUtility.UrlEncode(APIKey)));
            return strQuery.ToString();
        }

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
