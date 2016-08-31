using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCCMarketPlace.Model;

namespace TCCMarketPlace.Business.Interface
{
    public interface IMarketPlace : IDisposable
    {
        /// <summary>
        /// Gets the list of available services and return it to the API controller. 
        /// </summary>
        Task<MarketPlaceServiceList> GetMarketPlaceList(User currentUser, int typeId, string key = null);
        /// <summary>
        /// Gets the available categories and returns it to the API controller.
        /// </summary>
        List<Category> GetCategories(string userName, int typeId);
        /// <summary>
        /// Gets the details of the service selected by the user and returns it to the API controller.
        /// </summary>
        Task<Service> GetDetails(User user, int serviceId);
        /// <summary>
        /// Gets the list of slider image URLs and return it to the API controller. 
        /// </summary>
        Task<object> GetSlideShowImages(User currentUser, int typeId);
        /// <summary>
        /// Toggle the enabled status of the selected service.
        /// </summary>
        Task<Service> EnableOrDisableService(User currentUser, Service service);
        /// <summary>
        /// Unsubscribes a service for the user. Also un-enroll from service provider's database.
        /// </summary>
        Task<string> RemoveService(User currentUser, Service service);
        /// <summary>
        /// Saves the report URL generated on  enrolment.
        /// </summary>
        Task<string> SaveReportUrl(User currentUser, Service service);
        /// <summary>
        /// Subscribes a service for the user in case of free service.
        /// </summary>
        Task<string> SubscribeToService(User currentUser, Service service);
        /// <summary>
        /// Subscribes a service for the user in case of paid service.
        /// </summary>
        Task<string> SubscribeToService(UpdateTransactionRequest request);
        /// <summary>
        /// Initiates a transaction for a paid service. 
        /// </summary>
        Task<TransactionResponse> CreateInAppPurchaseTransaction(CreateTransactionRequest request);
        /// <summary>
        /// Update the transaction status to success or failure.
        /// </summary>
        Task<TransactionResponse> UpdateInAppPurchaseTransaction(UpdateTransactionRequest request);
        /// <summary>
        /// Gets the list of newest services and return it to the API controller.
        /// </summary>
        Task<MarketPlaceServiceList> GetNewlyAddedServices(User currentUser, int typeId, string key = null);
        /// <summary>
        /// Gets the list of subscribed services and return it to the API controller.
        /// </summary>
        Task<MarketPlaceServiceList> GetMyServices(User currentUser, int typeId, string key = null);
        /// <summary>
        /// Gets the latest transaction details for a user service combination and returns it to the API controller.
        /// </summary>
        Task<TransactionDetailsResponse> GetTransactionDetails(CreateTransactionRequest request);
        /// <summary>
        /// Generates the the marketplace details URL for the selected service.
        /// </summary>
        string GetDetailsUrl(int serviceId, int serviceTypeId);
        /// <summary>
        /// Checks if the selected service is already subscribed.
        /// </summary>
        Task<bool> IsServiceSubscribed(User currentUser, int serviceId);
    }
}
