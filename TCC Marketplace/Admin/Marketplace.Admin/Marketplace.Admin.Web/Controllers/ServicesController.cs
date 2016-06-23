using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Marketplace.Admin.Models;
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

namespace Marketplace.Admin.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {

        private readonly IServiceManager _serviceManager;
        private readonly ILocationManager _locationManager;
        private readonly IProductManager _productManager;

        public ServicesController(IServiceManager serviceManager, ILocationManager locationManager, IProductManager productManager)
        {
            _serviceManager = serviceManager;
            _locationManager = locationManager;
            _productManager = productManager;
        }

        // GET: Services
        public ActionResult Index(string country, string state, string keywords, string thermostats, string SCFs, string zipCodes, int? page)
        {
            ViewBag.CountryList = GetCountries();
            return View();
        }

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

        // GET: Services/Create
        public ActionResult Create()
        {
            ViewBag.CountryList = GetCountries();
            ViewBag.ProductCategories = new SelectList(_productManager.GetProductCategories(), "Id", "Name");
            ViewBag.ServiceTypeId = new SelectList(_serviceManager.GetServiceTypes(), "Id", "Description");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));

                // Breakpoint, Log or examine the list with Exceptions.

            }

            ViewBag.CountryList = GetCountries();
            ViewBag.ProductCategories = new SelectList(_productManager.GetProductCategories(), "Id", "Name");
            ViewBag.ServiceTypeId = new SelectList(_serviceManager.GetServiceTypes(), "Id", "Description", serviceViewModel.ServiceTypeId);
            return View(serviceViewModel);
        }
        
        private List<ProductViewModel> BuildProducts(List<string> product)
        {
            var productList = new List<ProductViewModel>();

            if (product != null)
            {
                productList.AddRange(product.Select(prod => JObject.Parse(HttpUtility.UrlDecode(prod)).ToObject<ProductViewModel>()));
            }

            return productList;
        }

        private List<LocationViewModel> BuildLocations(List<string> location)
        {
            var locationList = new List<LocationViewModel>();

            if (location != null)
            {
                locationList.AddRange(location.Select(loc => JObject.Parse(HttpUtility.UrlDecode(loc)).ToObject<LocationViewModel>()));
            }

            return locationList;
        }

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
                ThirdPartyAPI = serviceViewModel.ThirdPartyAPI,
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

                var blob = CreateCloudBlob(iconFile);
                blob.Properties.ContentType = iconFile.ContentType;
                blob.UploadByteArray(img.GetBytes());

                serviceDto.IconImage = blob.Uri.ToString();
            }

            if (sliderFile != null)
            {
                var sliderImage = new WebImage(sliderFile.InputStream);
                if (sliderImage.Width > 620 || sliderImage.Height > 250)
                    sliderImage.Resize(620, 250, true, false);

                var blob = CreateCloudBlob(sliderFile);
                blob.Properties.ContentType = sliderFile.ContentType;
                blob.UploadByteArray(sliderImage.GetBytes());

                serviceDto.SliderImage = blob.Uri.ToString();
            }

            return serviceDto;
        }

        private static CloudBlockBlob CreateCloudBlob(HttpPostedFileBase image)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
            var blobStorage = storageAccount.CreateCloudBlobClient();
            var container = blobStorage.GetContainerReference("mkplqa");
            if (container.CreateIfNotExist())
            {
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }

            var uniqueBlobName = $"mkplqa/image_{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

            return blobStorage.GetBlockBlobReference(uniqueBlobName);
        }

        // GET: Services/Edit/5
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
                MakeLive = service.MakeLive,
                StartDate = service.StartDate,
                EndDate = service.EndDate,
                PartnerPromoCode = service.PartnerPromoCode,
                PurchasePrice = service.PurchasePrice,
                ThirdPartyAPI = service.ThirdPartyAPI,
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
            ViewBag.ProductCategories = new SelectList(_productManager.GetProductCategories(), "Id", "Name");
            ViewBag.ServiceTypeId = new SelectList(_serviceManager.GetServiceTypes(), "Id", "Description", service.ServiceTypeId);

            return View(serviceViewModel);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceViewModel service, List<string> Location, List<string> Product, HttpPostedFileBase iconFile, HttpPostedFileBase sliderFile)
        {

            service.Locations = BuildLocations(Location);
            service.Products = BuildProducts(Product); ;

            if (ModelState.IsValid)
            {
                //var serviceDataModel = marketplaceService.GetService(service.Id);

                var serviceDataModel = BuildServiceDto(service, iconFile, sliderFile);

                _serviceManager.UpdateService(serviceDataModel);

                _serviceManager.SaveService();

                return RedirectToAction("Index");
            }

            ViewBag.CountryList = GetCountries();
            ViewBag.ProductCategories = new SelectList(_productManager.GetProductCategories(), "Id", "Name");
            ViewBag.ServiceTypeId = new SelectList(_serviceManager.GetServiceTypes(), "Id", "Description", service.ServiceTypeId);
            return View(service);
        }

        public JsonResult GetStates(int countryId)
        {
            var stateList = new SelectList(_locationManager.GetStates(countryId), "Id", "State_Name");
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSCFs(int stateId)
        {
            var scfList = new SelectList(_locationManager.GetScFsOfState(stateId), "Id", "DisplayText");
            return Json(scfList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProducts(byte catId)
        {
            var productList = new SelectList(_productManager.GetProducts(catId), "Id", "Name");
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServices(string country, string state, string keywords, string thermostats, string SCFs, string zipCodes, int? page)
        {
            var serviceList = GetServiceList(country, state, keywords, thermostats, SCFs, zipCodes, page);
            return Json(serviceList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewLocationModal(IEnumerable<LocationViewModel> locations)
        {
            ViewBag.CountryList = GetCountries();
            return PartialView("_ServiceLocation", locations);
        }

        private IEnumerable<SelectListItem> GetCountries()
        {
            if (TempData["Countries"] == null)
            {
                TempData["Countries"] = new SelectList(_locationManager.GetCountries(), "Id", "Country_Name");
            }

            return (IEnumerable<SelectListItem>)TempData["Countries"];
        }

        public JsonResult UpdateServiceStatus(int serviceId, bool flag)
        {
            var service = _serviceManager.GetService(serviceId);
            service.IsActive = flag;
            service.UpdatedDate = DateTime.Now;
            service.UpdatedUser = User.Identity.Name;
            _serviceManager.UpdateServiceStatus(service);
            _serviceManager.SaveService();
            return Json(service.IsActive, JsonRequestBehavior.AllowGet);
        }

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
    }
}
