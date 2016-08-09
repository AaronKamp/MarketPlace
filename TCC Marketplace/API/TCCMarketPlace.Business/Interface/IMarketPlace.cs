using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCCMarketPlace.Model;

namespace TCCMarketPlace.Business.Interface
{
    public interface IMarketPlace : IDisposable
    {
        Task<MarketPlaceDetails> GetMarketPlaceList(User currentUser, int typeId, string key);
        List<Category> GetCategories(string userName, int typeId);
        Task<Service> GetDetails(User user, int serviceId);
        Task<object> GetSlideShowImages(User currentUser, int typeId);
        Task<Service> EnableOrDisableService(User currentUser, Service service);
        Task<string> RemoveService(User currentUser, Service service);
        Task<string> SaveReportUrl(User currentUser, Service service);
        Task<string> SubscribeToService(User currentUser, Service service);
        Task<string> SubscribeToService(UpdateTransactionRequest request);
        Task<TransactionResponse> CreateInAppPurchaseTransaction(CreateTransactionRequest request);
        Task<TransactionResponse> UpdateInAppPurchaseTransaction(UpdateTransactionRequest request);
        Task<MarketPlaceDetails> GetNewlyAddedServices(User currentUser, int typeId, string key);
        Task<MarketPlaceDetails> GetMyServices(User currentUser, int typeId, string key);
        Task<TransactionDetailsResponse> GetTransactionDetails(CreateTransactionRequest request);
        string GetDetailsUrl(int serviceId, int serviceTypeId);
        Task<bool> IsServiceSubscribed(User currentUser, int serviceId);
    }
}
