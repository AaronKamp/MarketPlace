using System.Collections.Generic;
using Marketplace.Admin.Core.DTO;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
   public interface IServiceManager
    {
        /// <summary>
        /// Gets service pagination model filtered by the below details.
        /// </summary>
        ServicePaginationDTO GetServices(string country, string state, string keywords, string thermostats, string scFs, string zipCodes, int? page);

        /// <summary>
        /// Gets the list of service types.
        /// </summary>
        IList<ServiceType> GetServiceTypes();

        /// <summary>
        /// Creates new service.
        /// </summary>
        void CreateService(ServiceDTO serviceDto);

        /// <summary>
        /// Gets service by serviceId.
        /// </summary>
        Model.Service GetService(int id);

        /// <summary>
        /// Gets service by serviceId.
        /// </summary>
        Service GetDetailsById(int id);

        /// <summary>
        /// Updates a service.
        /// </summary>
        void UpdateService(ServiceDTO serviceDto);

        /// <summary>
        /// Removes service product mapping.
        /// </summary>
        void DeleteServiceProduct(ServiceProduct serviceProduct);

        /// <summary>
        /// Save database changes.
        /// </summary>
        void SaveService();

        /// <summary>
        /// Updates service status.
        /// </summary>
        void UpdateServiceStatus(Model.Service service);
    }
}
