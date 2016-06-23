using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Admin.Core.DTO;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    public class ServiceManager : IServiceManager
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IServiceProductRepository _serviceProductRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceManager(IServiceRepository serviceRepository,
                                    IServiceTypeRepository serviceTypeRepository,
                                    IServiceProductRepository serviceProductRepository,
                                    IUnitOfWork unitOfWork)
        {
            _serviceRepository = serviceRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _serviceProductRepository = serviceProductRepository;
            _unitOfWork = unitOfWork;
        }

        public Model.Service GetService(int id)
        {
            return _serviceRepository.GetById(id);
        }

        public Model.Service GetDetailsById(int id)
        {
            return _serviceRepository.GetDetailsById(id);
        }

        public ServicePaginationDTO GetServices(string country, string state, string keywords, string thermostats, string scFs, string zipCodes, int? page)
        {
            return _serviceRepository.GetServices(country, state, keywords, thermostats, scFs, zipCodes, page);
        }

        public IList<ServiceType> GetServiceTypes()
        {
            return _serviceTypeRepository.GetAll().ToList();
        }

        public void CreateService(ServiceDTO serviceDto)
        {
            if (serviceDto == null)
                throw new ArgumentNullException(nameof(serviceDto));

            var service = new Model.Service();

            MapService(serviceDto, service);

            service.CreatedDate = DateTime.Now;
            service.UpdatedDate = DateTime.Now;

            _serviceRepository.Add(service);
        }

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
            serviceDataModel.ThirdPartyAPI = serviceDto.ThirdPartyAPI;
            serviceDataModel.PartnerPromoCode = serviceDto.PartnerPromoCode;
            serviceDataModel.PurchasePrice = serviceDto.PurchasePrice;
            serviceDataModel.URL = serviceDto.URL;
            serviceDataModel.UpdatedUser = serviceDto.UpdatedUser;
            serviceDataModel.ZipCodes = serviceDto.ZipCodes;
            serviceDataModel.DisableAPIAvailable = serviceDto.DisableAPIAvailable;
            serviceDataModel.IconImage = serviceDto.IconImage ?? serviceDataModel.IconImage;
            serviceDataModel.SliderImage = serviceDto.SliderImage ?? serviceDataModel.SliderImage;
            serviceDataModel.UpdatedDate = DateTime.Now;
            

            ManageProducts(serviceDto, serviceDataModel);
            ManageScFs(serviceDto, serviceDataModel);
        }

        private void ManageProducts(ServiceDTO serviceDto, Model.Service existingService)
        {
            if (existingService.ServiceProducts == null)
                existingService.ServiceProducts = new List<ServiceProduct>();

            var existingProducts = existingService.ServiceProducts.ToList();

            var updatedProducts = serviceDto.Products.Select(p => new ServiceProduct
            {
                ProductId = p, Product = new Product {Id = p}, ServiceId = existingService.Id
            }).ToList();

            var deletedProducts = existingProducts.Where(p1 => updatedProducts.All(p2 => p2.ProductId != p1.ProductId));
            var addedProducts = updatedProducts.Where(p1 => existingProducts.All(p2 => p2.ProductId != p1.ProductId));

            foreach (var serProd in deletedProducts)
            {
                DeleteServiceProduct(serProd);
            }

            foreach (var prod in addedProducts)
            {
                existingService.ServiceProducts.Add(prod);
            }
        }

        private static void ManageScFs(ServiceDTO serviceDto, Model.Service existingService)
        {
            if (existingService.SCFs == null)
                existingService.SCFs = new List<SCF>();

            var existingScFs = existingService.SCFs.ToList();

            var updatedScFs = serviceDto.Locations.Select(scfId => new SCF {Id = scfId}).ToList();


            var deletedScFs = existingScFs.Where(s1 => updatedScFs.All(s2 => s2.Id != s1.Id));
            var addedScFs = updatedScFs.Where(s1 => existingScFs.All(s2 => s2.Id != s1.Id));

            foreach (var scf in deletedScFs)
            {
                existingService.SCFs.Remove(scf);
            }

            foreach (var scf in addedScFs)
            {
                existingService.SCFs.Add(scf);
            }
        }

        public void DeleteServiceProduct(ServiceProduct serviceProduct)
        {
            _serviceProductRepository.Delete(serviceProduct);
        }

        public void SaveService()
        {
            _unitOfWork.Commit();
        }

        public void UpdateServiceStatus(Model.Service service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            _serviceRepository.UpdateServiceStatus(service);
        }
    }
}
