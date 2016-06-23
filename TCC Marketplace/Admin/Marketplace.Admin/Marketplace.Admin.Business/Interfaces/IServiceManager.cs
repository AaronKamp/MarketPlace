using System.Collections.Generic;
using Marketplace.Admin.Core.DTO;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
   public interface IServiceManager
    {
        ServicePaginationDTO GetServices(string country, string state, string keywords, string thermostats, string scFs, string zipCodes, int? page);
        IList<ServiceType> GetServiceTypes();
        void CreateService(ServiceDTO serviceDto);
        Model.Service GetService(int id);
        void UpdateService(ServiceDTO serviceDto);
        void DeleteServiceProduct(ServiceProduct serviceProduct);
        void SaveService();
        void UpdateServiceStatus(Model.Service service);
        Service GetDetailsById(int id);
    }
}
