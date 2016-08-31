using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Admin.Core.DTO;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    /// <summary>
    /// Business logic for all services
    /// </summary>
    public class ServiceManager : IServiceManager
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IServiceProductRepository _serviceProductRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageQueueRepository _imageQueue;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="serviceRepository"></param>
        /// <param name="serviceTypeRepository"></param>
        /// <param name="serviceProductRepository"></param>
        /// <param name="imageQueue"></param>
        /// <param name="unitOfWork"></param>
        public ServiceManager(IServiceRepository serviceRepository,
                                    IServiceTypeRepository serviceTypeRepository,
                                    IServiceProductRepository serviceProductRepository,
                                    IImageQueueRepository imageQueue,
                                    IUnitOfWork unitOfWork)
        {
            _serviceRepository = serviceRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _serviceProductRepository = serviceProductRepository;
            _imageQueue = imageQueue;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets service by serviceId.
        /// </summary>
        /// <param name="id"> ServiceId</param>
        /// <returns> Service Details.</returns>
        public Model.Service GetService(int id)
        {
            return _serviceRepository.GetById(id);
        }

        /// <summary>
        /// Gets service by serviceId.
        /// </summary>
        /// <param name="id"> ServiceId</param>
        /// <returns> Service Details.</returns>
        public Model.Service GetDetailsById(int id)
        {
            return _serviceRepository.GetDetailsById(id);
        }

        /// <summary>
        /// Gets service pagination model filtered by the below details.
        /// </summary>
        /// <param name="country"> Country.</param>
        /// <param name="state"> State.</param>
        /// <param name="keywords"> Keyword. </param>
        /// <param name="thermostats"> Thermostat. </param>
        /// <param name="scFs"> SCF </param>
        /// <param name="zipCodes"> Zip code. </param>
        /// <param name="page"> Page no.</param>
        /// <returns> Service List after filtering.</returns>
        public ServicePaginationDTO GetServices(string country, string state, string keywords, string thermostats, string scFs, string zipCodes, int? page)
        {
            return _serviceRepository.GetServices(country, state, keywords, thermostats, scFs, zipCodes, page);
        }

        /// <summary>
        /// Gets the list of service types.
        /// </summary>
        /// <returns>List of service types. </returns>
        public IList<ServiceType> GetServiceTypes()
        {
            return _serviceTypeRepository.GetAll().ToList();
        }

        /// <summary>
        /// Creates new service.
        /// </summary>
        /// <param name="serviceDto"> Service data transfer object.</param>
        public void CreateService(ServiceDTO serviceDto)
        {
            if (serviceDto == null)
                throw new ArgumentNullException(nameof(serviceDto));

            var service = new Model.Service();
            
            //Maps Service details in serviceDto to service.
            MapService(serviceDto, service);

            service.CreatedDate = DateTime.UtcNow;
            service.UpdatedDate = DateTime.UtcNow;

            _serviceRepository.Add(service);
        }

        /// <summary>
        /// Updates a service.
        /// </summary>
        /// <param name="serviceDto"> Service data transfer object. </param>
        public void UpdateService(ServiceDTO serviceDto)
        {
            if (serviceDto == null)
                throw new ArgumentNullException(nameof(serviceDto));

            var existingService = _serviceRepository.GetById(serviceDto.Id);

            if (existingService == null)
                throw new NullReferenceException("Unable to find specified service");

            MapService(serviceDto, existingService);

            _serviceRepository.Update(existingService);
        }

        /// <summary>
        /// Maps Service details in serviceDto to service.
        /// </summary>
        /// <param name="serviceDto"> Service Data transfer object. </param>
        /// <param name="serviceDataModel"> Service domain model.  </param>
        private void MapService(ServiceDTO serviceDto, Model.Service serviceDataModel)
        {
            serviceDataModel.Tilte = serviceDto.Tilte;
            serviceDataModel.ShortDescription = serviceDto.ShortDescription;
            serviceDataModel.LongDescription = serviceDto.LongDescription;
            serviceDataModel.ServiceTypeId = serviceDto.ServiceTypeId;
            serviceDataModel.IsActive = serviceDto.IsActive;
            serviceDataModel.InAppPurchaseId = serviceDto.InAppPurchaseId;
            serviceDataModel.CustomField1 = serviceDto.CustomField1;
            serviceDataModel.CustomField2 = serviceDto.CustomField2;
            serviceDataModel.CustomField3 = serviceDto.CustomField3;
            serviceDataModel.ServiceStatusAPIAvailable = serviceDto.ServiceStatusAPIAvailable;
            serviceDataModel.MakeLive = serviceDto.MakeLive;
            serviceDataModel.StartDate = serviceDto.StartDate;
            serviceDataModel.EndDate = serviceDto.EndDate;
            serviceDataModel.ServiceProviderId = serviceDto.ServiceProviderId;
            serviceDataModel.PartnerPromoCode = serviceDto.PartnerPromoCode;
            serviceDataModel.PurchasePrice = serviceDto.PurchasePrice;
            serviceDataModel.URL = serviceDto.URL;
            serviceDataModel.UpdatedUser = serviceDto.UpdatedUser;
            serviceDataModel.ZipCodes = serviceDto.ZipCodes;
            serviceDataModel.DisableAPIAvailable = serviceDto.DisableAPIAvailable;
            serviceDataModel.IconImage = serviceDto.IconImage ?? serviceDataModel.IconImage;
            if (serviceDto.SliderImage != serviceDataModel.SliderImage)
            {
                if (serviceDataModel.SliderImage != null)
                    QueueImageForDelete(serviceDataModel.SliderImage, serviceDto.UpdatedUser);

                serviceDataModel.SliderImage = serviceDto.SliderImage;
            }
            serviceDataModel.UpdatedDate = DateTime.UtcNow;

            ManageProducts(serviceDto, serviceDataModel);
            ManageScFs(serviceDto, serviceDataModel);
        }

        /// <summary>
        /// Queue deleted slider images.
        /// </summary>
        /// <param name="existingImageUrl"> </param>
        /// <param name="deletedUser"> Deleted User.</param>
        private void QueueImageForDelete(string existingImageUrl, string deletedUser)
        {
            _imageQueue.Add(new ImageQueue
            {
                ImageUrl = existingImageUrl,
                DeletedUser = deletedUser,
                ActualDeletedDate = DateTime.UtcNow,
                PurgedDate = null
            });
        }

        /// <summary>
        /// Manage product mapping for a service.
        /// </summary>
        /// <param name="serviceDto">new Service DTO </param>
        /// <param name="existingService"> Service domain object being updated. </param>
        private void ManageProducts(ServiceDTO serviceDto, Model.Service existingService)
        {
            if (existingService.ServiceProducts == null)
                existingService.ServiceProducts = new List<ServiceProduct>();

            var existingProducts = existingService.ServiceProducts.ToList();

            var updatedProducts = serviceDto.Products.Select(p => new ServiceProduct
            {
                ProductId = p,
                Product = new Product { Id = p },
                ServiceId = existingService.Id
            }).ToList();

            var deletedProducts = existingProducts.Where(p1 => updatedProducts.All(p2 => p2.ProductId != p1.ProductId));
            var addedProducts = updatedProducts.Where(p1 => existingProducts.All(p2 => p2.ProductId != p1.ProductId));

            foreach (var serProd in deletedProducts)
            {
                //Remove mapping for deleted products.
                DeleteServiceProduct(serProd);
            }

            foreach (var prod in addedProducts)
            {
                //Adds mapping for new products
                existingService.ServiceProducts.Add(prod);
            }
        }

        /// <summary>
        /// Manage location mapping for a service.
        /// </summary>
        /// <param name="serviceDto">new Service DTO </param>
        /// <param name="existingService"> Service domain object being updated. </param>
        private static void ManageScFs(ServiceDTO serviceDto, Model.Service existingService)
        {
            if (existingService.SCFs == null)
                existingService.SCFs = new List<SCF>();

            var existingScFs = existingService.SCFs.ToList();

            var updatedScFs = serviceDto.Locations.Select(scfId => new SCF { Id = scfId }).ToList();


            var deletedScFs = existingScFs.Where(s1 => updatedScFs.All(s2 => s2.Id != s1.Id));
            var addedScFs = updatedScFs.Where(s1 => existingScFs.All(s2 => s2.Id != s1.Id));

            foreach (var scf in deletedScFs)
            {
                //Remove mapping for deleted locations.
                existingService.SCFs.Remove(scf);
            }

            foreach (var scf in addedScFs)
            {
                //Adds mapping for new locations.
                existingService.SCFs.Add(scf);
            }
        }

        /// <summary>
        /// Removes service product mapping.
        /// </summary>
        /// <param name="serviceProduct"> service product mapping model.</param>
        public void DeleteServiceProduct(ServiceProduct serviceProduct)
        {
            _serviceProductRepository.Delete(serviceProduct);
        }

        /// <summary>
        /// Save database changes.
        /// </summary>
        public void SaveService()
        {
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Updates service status.
        /// </summary>
        /// <param name="service"></param>
        public void UpdateServiceStatus(Model.Service service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            _serviceRepository.UpdateServiceStatus(service);
        }
    }
}
