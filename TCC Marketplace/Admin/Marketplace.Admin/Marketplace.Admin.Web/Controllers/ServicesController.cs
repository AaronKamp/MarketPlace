using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Web;
using System.Web.Helpers;
using System.IO;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using System.Configuration;
using Marketplace.Admin.Business;
using Marketplace.Admin.Core.DTO;
using Marketplace.Admin.ViewModels;
using Marketplace.Admin.Enums;
using Marketplace.Admin.Utils;

namespace Marketplace.Admin.Controllers
{
    /// <summary>
    /// Controls Services manipulation operations.
    /// </summary>
    [Authorize]
    public class ServicesController : Controller
    {
        private const string imageDirectory = "images";
        private readonly IServiceManager _serviceManager;
        private readonly ILocationManager _locationManager;
        private readonly IProductManager _productManager;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="serviceManager"></param>
        /// <param name="locationManager"></param>
        /// <param name="productManager"></param>
        public ServicesController(IServiceManager serviceManager, ILocationManager locationManager, IProductManager productManager)
        {
            _serviceManager = serviceManager;
            _locationManager = locationManager;
            _productManager = productManager;
        }

        /// <summary>
        /// Returns service list page.
        /// GET: Services
        /// </summary>
        /// <param name="country"> Country name.</param>
        /// <param name="state"> State name.</param>
        /// <param name="keywords"> Search key.</param>
        /// <param name="thermostats"> Thermostat Id.</param>
        /// <param name="SCFs"> SCF name.</param>
        /// <param name="zipCodes">ZIP code. </param>
        /// <param name="page"> Page no.</param>
        /// <returns> List of Service filtered by above details.</returns>
        public ActionResult Index(string country, string state, string keywords, string thermostats, string SCFs, string zipCodes, int? page)
        {
            ViewBag.CountryList = GetCountries();
            return View();
        }

        /// <summary>
        /// Returns service list page.
        /// </summary>
        /// <param name="country"> Country name.</param>
        /// <param name="state"> State name.</param>
        /// <param name="keywords"> Search key.</param>
        /// <param name="thermostats"> Thermostat Id.</param>
        /// <param name="SCFs"> SCF name.</param>
        /// <param name="zipCodes">ZIP code. </param>
        /// <param name="page"> Page no.</param>
        /// <returns> List of Service filtered by above details.</returns>
        private ServiceGridPaginationModel GetServiceList(string country, string state, string keywords, string thermostats, string SCFs, string zipCodes, int? page)
        {
            var pageNo = page ?? 1;

            var serviceGridPaginationDto = _serviceManager.GetServices(country, state, keywords, thermostats, SCFs, zipCodes, pageNo);

            var rowNo = ((serviceGridPaginationDto.CurrentPage - 1) * serviceGridPaginationDto.PageSize) + 1;
            var sgvm = new ServiceGridPaginationModel()
            {
                PageSize = serviceGridPaginationDto.PageSize,
                TotalRecord = serviceGridPaginationDto.TotalRecord,
                NoOfPages = serviceGridPaginationDto.NoOfPages,
                CurrentPage = serviceGridPaginationDto.CurrentPage,
                Services = (from s in serviceGridPaginationDto.Services
                            select new ServiceGridViewModel
                            {
                                RowNo = rowNo++,
                                ServiceId = s.ServiceId,
                                Countries = s.Countries,
                                EndDate = s.EndDate,
                                IconImage = s.IconImage,
                                IsActive = s.IsActive,
                                ProductId = s.ProductId,
                                ShortDescription = s.ShortDescription,
                                StartDate = s.StartDate,
                                States = s.States,
                                Title = s.Title,
                                Type = s.Type
                            }).ToList()
            };
            return sgvm;
        }

        /// <summary>
        /// Gets Add new service page.
        /// GET: Services/Create
        /// </summary>
        /// <returns> Create Service View.</returns>
        public ActionResult Create()
        {
            ViewBag.CountryList = GetCountries();
            ViewBag.ServiceProviderNameList = GetServiceProviderNameList();
            ViewBag.ProductCategories = new SelectList(_productManager.GetProductCategories(), "Id", "Name");
            ViewBag.ServiceTypeId = new SelectList(_serviceManager.GetServiceTypes(), "Id", "Description");
            return View();
        }

        /// <summary>
        /// Adds new services.
        /// POST: Services/Create
        /// </summary>
        /// <param name="serviceViewModel"> ServiceViewModel </param>
        /// <param name="Location"> List of locations.</param>
        /// <param name="Product"> List of products.</param>
        /// <param name="iconFile"> Icon image file for the service. </param>
        /// <param name="sliderFile"> Slider image file for the service.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceViewModel serviceViewModel, List<string> Location, List<string> Product, HttpPostedFileBase iconFile, HttpPostedFileBase sliderFile)
        {

            serviceViewModel.Locations = BuildLocations(Location);
            serviceViewModel.Products = BuildProducts(Product); ;

            if (ModelState.IsValid)
            {
                var serviceDto =  BuildServiceDto(serviceViewModel, iconFile, sliderFile);

                _serviceManager.CreateService(serviceDto);

                _serviceManager.SaveService();

                return RedirectToAction("Index");
            }

            ViewBag.CountryList = GetCountries();
            ViewBag.ServiceProviderNameList = GetServiceProviderNameList();
            ViewBag.ProductCategories = new SelectList(_productManager.GetProductCategories(), "Id", "Name");
            ViewBag.ServiceTypeId = new SelectList(_serviceManager.GetServiceTypes(), "Id", "Description", serviceViewModel.ServiceTypeId);
            return View(serviceViewModel);
        }

        
        /// <summary>
        /// Build Product list view model.
        /// </summary>
        /// <param name="product"> List of product </param>
        /// <returns> ProductViewModel List</returns>
        private List<ProductViewModel> BuildProducts(List<string> product)
        {
            var productList = new List<ProductViewModel>();

            if (product != null)
            {
                productList.AddRange(product.Select(prod => JObject.Parse(HttpUtility.UrlDecode(prod)).ToObject<ProductViewModel>()));
            }

            return productList;
        }

        /// <summary>
        /// Build location list view model.
        /// </summary>
        /// <param name="location"> List of location string.</param>
        /// <returns>LocationViewmodel List</returns>
        private List<LocationViewModel> BuildLocations(List<string> location)
        {
            var locationList = new List<LocationViewModel>();

            if (location != null)
            {
                locationList.AddRange(location.Select(loc => JObject.Parse(HttpUtility.UrlDecode(loc)).ToObject<LocationViewModel>()));
            }

            return locationList;
        }

        /// <summary>
        /// Build Service data transfer object.
        /// </summary>
        /// <param name="serviceViewModel"> ServiceViewModel</param>
        /// <param name="iconFile"> Icon Image</param>
        /// <param name="sliderFile">Slider Image </param>
        /// <returns></returns>
        private ServiceDTO BuildServiceDto(ServiceViewModel serviceViewModel, HttpPostedFileBase iconFile, HttpPostedFileBase sliderFile)
        {
            var serviceDto = new ServiceDTO
            {
                Id = serviceViewModel.Id,
                Tilte = serviceViewModel.Tilte,
                ShortDescription = serviceViewModel.ShortDescription,
                LongDescription = serviceViewModel.LongDescription,
                ServiceTypeId = serviceViewModel.ServiceTypeId,
                IsActive = serviceViewModel.Id == 0 || serviceViewModel.IsActive,
                InAppPurchaseId = serviceViewModel.InAppPurchaseId,
                CustomField1 = serviceViewModel.CustomField1,
                CustomField2 = serviceViewModel.CustomField2,
                CustomField3 = serviceViewModel.CustomField3,
                ServiceStatusAPIAvailable = serviceViewModel.ServiceStatusAPIAvailable,
                MakeLive = serviceViewModel.MakeLive,
                StartDate = serviceViewModel.StartDate,
                EndDate = serviceViewModel.EndDate.AddHours(23).AddMinutes(59).AddSeconds(59),
                ServiceProviderId = serviceViewModel.ServiceProviderId,
                PartnerPromoCode = serviceViewModel.PartnerPromoCode,
                PurchasePrice = serviceViewModel.PurchasePrice,
                URL = serviceViewModel.URL,
                UpdatedUser = User.Identity.Name,
                ZipCodes = serviceViewModel.ZipCodes,
                DisableAPIAvailable = serviceViewModel.DisableAPIAvailable
            };
            
            foreach (var prodList in serviceViewModel.Products)
            {
               foreach(var prod in prodList.ProductIds)
                {
                    serviceDto.Products.Add(prod);
                }
            }

            foreach (var locList in serviceViewModel.Locations)
            {
                foreach (var scf in locList.SCFs)
                {
                    serviceDto.Locations.Add(scf);
                }
            }

            if (iconFile != null)
            {
                var img = new WebImage(iconFile.InputStream);
                if (img.Width > 130 || img.Height > 130)
                    img.Resize(130, 130, true, false);

                var blob = CreateCloudBlob(iconFile,ImageType.Icon);
                blob.Properties.ContentType = iconFile.ContentType;
                blob.UploadByteArray(img.GetBytes());

                serviceDto.IconImage = blob.Uri.ToString();
            }

            if (sliderFile != null)
            {
                var sliderImage = new WebImage(sliderFile.InputStream);
                if (sliderImage.Width > 620 || sliderImage.Height > 250)
                    sliderImage.Resize(620, 250, true, false);

                var blob = CreateCloudBlob(sliderFile,ImageType.Slider);
                blob.Properties.ContentType = sliderFile.ContentType;
                blob.UploadByteArray(sliderImage.GetBytes());

                serviceDto.SliderImage = blob.Uri.ToString();
            }
            else if(serviceViewModel.SliderImage !=null)
            {
                serviceDto.SliderImage = serviceViewModel.SliderImage;
            }
            return serviceDto;
        }

        /// <summary>
        /// Create cloud block blob
        /// </summary>
        /// <param name="image"> Image File </param>
        /// <param name="type"> Image type. </param>
        /// <returns> CloudBlockBlob </returns>
        private static CloudBlockBlob CreateCloudBlob(HttpPostedFileBase image,ImageType type)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
            var blobStorage = storageAccount.CreateCloudBlobClient();
            var container = blobStorage.GetContainerReference(ConfigurationManager.AppSettings["Application.Environment"].ToLower());
            if (container.CreateIfNotExist())
            {
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }
            var directory = container.GetDirectoryReference($"{imageDirectory.ToLower()}/{type.ToString().ToLower()}");
            var uniqueBlobName = $"{ConfigurationManager.AppSettings["Application.Environment"].ToLower()}/{imageDirectory.ToLower()}/{type.ToString().ToLower()}/image_{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

            return blobStorage.GetBlockBlobReference(uniqueBlobName);
        }

        /// <summary>
        /// Returns edit a service view.
        /// GET: Services/Edit/5
        /// </summary>
        /// <param name="id"> Service Id </param>
        /// <returns> Edit view.</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var service = _serviceManager.GetDetailsById((int)id);

            if (service == null)
            {
                return HttpNotFound();
            }

            var serviceViewModel = new ServiceViewModel
            {
                Id = service.Id,
                Tilte = service.Tilte,
                ShortDescription = service.ShortDescription,
                LongDescription = service.LongDescription,
                ServiceTypeId = service.ServiceTypeId,
                IsActive = service.IsActive,
                SliderImage = service.SliderImage,
                IconImage = service.IconImage,
                InAppPurchaseId = service.InAppPurchaseId,
                CustomField1 = service.CustomField1,
                CustomField2 = service.CustomField2,
                CustomField3 = service.CustomField3,
                ServiceStatusAPIAvailable = service.ServiceStatusAPIAvailable,
                DisableAPIAvailable =service.DisableAPIAvailable,
                MakeLive = service.MakeLive,
                StartDate = service.StartDate,
                EndDate = service.EndDate,
                PartnerPromoCode = service.PartnerPromoCode,
                PurchasePrice = service.PurchasePrice,
                ServiceProviderId = service.ServiceProviderId,
                URL = service.URL,
                ZipCodes = service.ZipCodes
            };

            var productViewModelList = new List<ProductViewModel>();
            foreach (var prod in service.ServiceProducts)
            {
                var pvm = productViewModelList.Find(x => x.CategoryId == prod.Product.ProductCategoryId);
                if (pvm == null)
                {
                    pvm = new ProductViewModel
                    {
                        CategoryId = prod.Product.ProductCategoryId,
                        CategoryName = prod.Product.ProductCategory.Name,
                        ProductIds = new List<int> { prod.Product.Id },
                        ProductNames = prod.Product.Name
                    };

                    productViewModelList.Add(pvm);
                }
                else
                {
                    pvm.ProductIds.Add(prod.Product.Id);
                    pvm.ProductNames = pvm.ProductNames + ", " + prod.Product.Name;
                }

            }

            var locationViewModelList = new List<LocationViewModel>();
            foreach (var scf in service.SCFs)
            {
                var lvm = locationViewModelList.Find(x => x.StateId == scf.StateId);
                if (lvm == null)
                {
                    lvm = new LocationViewModel
                    {
                        CountryId = scf.State.CountryId,
                        CountryName = scf.State.Country.Country_Name,
                        StateId = scf.StateId,
                        StateName = scf.State.State_Name,
                        SCFs = new List<int> { scf.Id },
                        SCFNames = scf.DisplayText
                    };

                    locationViewModelList.Add(lvm);
                }
                else
                {
                    lvm.SCFs.Add(scf.Id);
                    lvm.SCFNames = lvm.SCFNames + ", " + scf.DisplayText;
                }

            }

            serviceViewModel.Locations = locationViewModelList;
            serviceViewModel.Products = productViewModelList;

            ViewBag.CountryList = GetCountries();
            int serviceProviderId;
            ViewBag.ServiceProviderNameList = int.TryParse(service.ServiceProviderId, out serviceProviderId) ? GetServiceProviderNameList(serviceProviderId) : GetServiceProviderNameList();
            ViewBag.ProductCategories = new SelectList(_productManager.GetProductCategories(), "Id", "Name");
            ViewBag.ServiceTypeId = new SelectList(_serviceManager.GetServiceTypes(), "Id", "Description", service.ServiceTypeId);

            return View(serviceViewModel);
        }

        /// <summary>
        /// Handles Service editing.
        /// POST: Services/Edit/5
        /// </summary>
        /// <param name="service">ServiceViewModel</param>
        /// <param name="Location"> Location List</param>
        /// <param name="Product"> product List</param>
        /// <param name="iconFile"> Icon file</param>
        /// <param name="sliderFile"> slider file.</param>
        /// <returns> Services List View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceViewModel service, List<string> Location, List<string> Product, HttpPostedFileBase iconFile, HttpPostedFileBase sliderFile)
        {

            service.Locations = BuildLocations(Location);
            service.Products = BuildProducts(Product); ;

            if (ModelState.IsValid)
            {
                //convert view model to service DTO
                var serviceDataModel = BuildServiceDto(service, iconFile, sliderFile);

                _serviceManager.UpdateService(serviceDataModel);

                _serviceManager.SaveService();

                return RedirectToAction("Index");
            }

            ViewBag.CountryList = GetCountries();
            int serviceProviderId;
            ViewBag.ServiceProviderNameList = int.TryParse(service.ServiceProviderId,out serviceProviderId)?GetServiceProviderNameList(serviceProviderId): GetServiceProviderNameList();
            ViewBag.ProductCategories = new SelectList(_productManager.GetProductCategories(), "Id", "Name");
            ViewBag.ServiceTypeId = new SelectList(_serviceManager.GetServiceTypes(), "Id", "Description", service.ServiceTypeId);
            return View(service);
        }

        /// <summary>
        /// Gets State list on country selection.
        /// </summary>
        /// <param name="countryId"> Country Id.</param>
        /// <returns> StateList. </returns>
        public JsonResult GetStates(int countryId)
        {
            var stateList = new SelectList(_locationManager.GetStates(countryId), "Id", "State_Name");
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets SCFlist of selected state.
        /// </summary>
        /// <param name="stateId"> State Id</param>
        /// <returns> SCF List</returns>
        public JsonResult GetSCFs(int stateId)
        {
            var scfList = new SelectList(_locationManager.GetScFsOfState(stateId), "Id", "DisplayText");
            return Json(scfList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets product list.
        /// </summary>
        /// <param name="catId"> Product category Id.</param>
        /// <returns> Product list.</returns>
        public JsonResult GetProducts(byte catId)
        {
            var productList = new SelectList(_productManager.GetProducts(catId), "Id", "Name");
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets Service list filtered by following details. 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="state"></param>
        /// <param name="keywords"></param>
        /// <param name="thermostats"></param>
        /// <param name="SCFs"></param>
        /// <param name="zipCodes"></param>
        /// <param name="page"></param>
        /// <returns> Services List</returns>
        public JsonResult GetServices(string country, string state, string keywords, string thermostats, string SCFs, string zipCodes, int? page)
        {
            var serviceList = GetServiceList(country, state, keywords, thermostats, SCFs, zipCodes, page);
            return Json(serviceList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the list of countries.
        /// </summary>
        /// <returns> Country List</returns>
        private IEnumerable<SelectListItem> GetCountries()
        {
            if (TempData["Countries"] == null)
            {
                TempData["Countries"] = new SelectList(_locationManager.GetCountries(), "Id", "Country_Name");
            }

            return (IEnumerable<SelectListItem>)TempData["Countries"];
        }

        /// <summary>
        /// Updates service activated status.
        /// </summary>
        /// <param name="serviceId"> Service Id.</param>
        /// <param name="flag"> New status.</param>
        /// <returns> service.IsActive.</returns>
        public JsonResult UpdateServiceStatus(int serviceId, bool flag)
        {
            var service = _serviceManager.GetService(serviceId);
            service.IsActive = flag;
            service.UpdatedDate = DateTime.UtcNow;
            service.UpdatedUser = User.Identity.Name;
            _serviceManager.UpdateServiceStatus(service);
            _serviceManager.SaveService();
            return Json(service.IsActive, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets SCF list on country selection.
        /// </summary>
        /// <param name="countryId"> Selected Country Id/</param>
        /// <returns> SCF List </returns>
        public JsonResult LoadSCFs(int countryId)
        {
            var SCFs = _locationManager.GetAllScFs(countryId);

            var locationViewModelList = new List<LocationViewModel>();
            foreach (var scf in SCFs)
            {
                var loc = locationViewModelList.Find(x => x.StateId == scf.StateId);
                if (loc == null)
                {
                    loc = new LocationViewModel
                    {
                        CountryId = scf.State.CountryId,
                        CountryName = scf.State.Country.Country_Name,
                        StateId = scf.StateId,
                        StateName = scf.State.State_Name,
                        SCFs = new List<int> { scf.Id },
                        SCFNames = scf.DisplayText
                    };

                    locationViewModelList.Add(loc);
                }
                else
                {
                    loc.SCFs.Add(scf.Id);
                    loc.SCFNames = loc.SCFNames + "," + scf.DisplayText;
                }
            }

            return Json(locationViewModelList.ToList(), JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Gets the signup URL for a selected service provider.
        /// </summary>
        /// <param name="id"> Service Provider Id.</param>
        /// <returns> Signup URL.</returns>
        public JsonResult GetSignUpUrl(int id)
        {
            string signUpUrl = ServiceProviderManager.GetSignUpUrl(id);
            return Json(signUpUrl,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets Service provider name drop down list 
        /// </summary>
        /// <returns>Service provider name drop down list </returns>
        private IEnumerable<SelectListItem> GetServiceProviderNameList()
        {
            return new SelectList(ServiceProviderManager.GetActiveServiceProviderList(), "Id", "Name");
        }

        /// <summary>
        /// Gets Service provider name drop down list 
        /// </summary>
        /// <param name="id"> Selected service provider id.</param>
        /// <returns>Service provider name drop down list </returns>
        private IEnumerable<SelectListItem> GetServiceProviderNameList(int id)
        {
            return new SelectList(ServiceProviderManager.GetActiveServiceProviderList(), "Id", "Name", id);
        }
    }
}
