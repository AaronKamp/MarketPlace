using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Marketplace.Admin.ViewModels;
using Marketplace.Admin.Utils;
using Marketplace.Admin.Core;

namespace Marketplace.Admin.Controllers
{
    // To be reworked with DB implementation in the next phase 
    [Authorize]
    public class ServiceProviderController : Controller
    {
        // GET: ServiceProvider
        public ActionResult Index()
        {
            var list = new Dictionary<int, string>();

            ViewBag.ProviderList = GetServiceProviderNameList();

            return View(new ServiceProviderViewModel());
        }


        //public List<ServiceProviderViewModel> GetList()
        //{
        //    var blob = GetCloudBlockBlob();
        //    var xml = blob.DownloadText();
        //    var xDoc = XDocument.Parse(xml);

        //}
        public ActionResult Save(ServiceProviderViewModel serviceProviderVM)
        {
            if (ModelState.IsValid)
            {
                serviceProviderVM.UpdatedDate = DateTime.Now;
                serviceProviderVM.UpdatedUser = User.Identity.Name;

                if (serviceProviderVM.Id > 0)
                {
                    UpdateServiceProvider(serviceProviderVM);
                }
                else
                {
                    if (IsDuplicate(serviceProviderVM.Name))
                    {
                        this.ModelState.AddModelError("Name", "Provider name "+ serviceProviderVM.Name + " is duplicate");
                        ViewBag.ProviderList = GetServiceProviderNameList(serviceProviderVM.Id);
                        return View("Index", serviceProviderVM);
                    }
                    else {
                        serviceProviderVM.CreatedDate = DateTime.Now;
                        AddServiceProvider(serviceProviderVM);
                    }
                }
                TempData["ResponseMessage"] = "Service Provider Successfully added"; 
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProviderList = GetServiceProviderNameList(serviceProviderVM.Id);
                return View("Index", serviceProviderVM);
            }

        }

        private bool IsDuplicate(string serviceProviderName)
        {
          return ServiceProviderManager.GetServiceProviderList()
                .Where(p => p.Name == serviceProviderName).Any();
        }

        public PartialViewResult Details(int id)
        {
            var serviceProviderViewModel = new ServiceProviderViewModel();
            if (id > 0)
            {
                serviceProviderViewModel = GetServiceProviderDetailsById(id);
            }

            return PartialView("_Edit", serviceProviderViewModel);
        }

        private ServiceProviderViewModel GetServiceProviderDetailsById(int id)
        {
            
            return ServiceProviderManager.GetServiceProviderList().Where(p=>p.Id == id).FirstOrDefault();
        }



        private void AddServiceProvider(ServiceProviderViewModel serviceProviderViewModel)
        {
            var blob = ServiceProviderManager.GetCloudBlockBlob();
            var document = ServiceProviderManager.GetServiceProviderXml(blob);
            var Id = document.Descendants("ServiceProvider").Any() ?
                document.Descendants("ServiceProvider").Select(p => Convert.ToInt32(p.Element("Id").Value)).ToList().Max() + 1 : 1;
            XElement provider = new XElement("ServiceProvider",
                                        new XElement("Id", Id),
                                        new XElement("Name", serviceProviderViewModel.Name),
                                        new XElement("SignUpUrl", serviceProviderViewModel.SignUpUrl),
                                        new XElement("StatusUrl", serviceProviderViewModel.StatusUrl),
                                        new XElement("UnEnrollUrl", serviceProviderViewModel.UnEnrollUrl),
                                        new XElement("GenerateBearerToken", serviceProviderViewModel.GenerateBearerToken.ToString()),
                                        new XElement("IsActive", serviceProviderViewModel.IsActive.ToString()),
                                        new XElement("IsDeleted", serviceProviderViewModel.IsDeleted.ToString()),
                                        new XElement("TokenUrl", serviceProviderViewModel.TokenUrl),
                                        new XElement("AppId", string.IsNullOrWhiteSpace(serviceProviderViewModel.AppId) ? string.Empty: Cryptography.EncryptContent(serviceProviderViewModel.AppId)),
                                        new XElement("SecretKey", string.IsNullOrWhiteSpace(serviceProviderViewModel.SecretKey) ? string.Empty : Cryptography.EncryptContent(serviceProviderViewModel.SecretKey)),
                                        new XElement("CreatedDate", serviceProviderViewModel.CreatedDate.ToString()),
                                        new XElement("UpdatedDate", serviceProviderViewModel.UpdatedDate.ToString()),
                                        new XElement("UpdatedUser", serviceProviderViewModel.UpdatedUser)
                                        );
            document.Root.Add(provider);
            blob.UploadText(document.ToString());
        }
        private void UpdateServiceProvider(ServiceProviderViewModel serviceProviderVM)
        {
            var blob = ServiceProviderManager.GetCloudBlockBlob();
            var document = ServiceProviderManager.GetServiceProviderXml(blob);
            XElement provider = document.Descendants("ServiceProvider")
                    .FirstOrDefault(p => Convert.ToInt32(p.Element("Id").Value) == serviceProviderVM.Id);
            if (provider == null)
            {
                throw new KeyNotFoundException("Unable to find a student matching the key");
            }
            else
            {
                provider.Element("Name").Value = serviceProviderVM.Name;
                provider.Element("SignUpUrl").Value = serviceProviderVM.SignUpUrl;
                provider.Element("StatusUrl").Value = serviceProviderVM.StatusUrl??string.Empty;
                provider.Element("UnEnrollUrl").Value = serviceProviderVM.UnEnrollUrl??string.Empty;
                provider.Element("GenerateBearerToken").Value = serviceProviderVM.GenerateBearerToken.ToString();
                provider.Element("IsActive").Value = serviceProviderVM.IsActive.ToString();
                provider.Element("IsDeleted").Value = serviceProviderVM.IsDeleted.ToString();
                provider.Element("TokenUrl").Value = serviceProviderVM.TokenUrl??string.Empty;
                provider.Element("AppId").Value = string.IsNullOrWhiteSpace(serviceProviderVM.AppId) ? string.Empty : Cryptography.EncryptContent(serviceProviderVM.AppId);
                provider.Element("SecretKey").Value = string.IsNullOrWhiteSpace(serviceProviderVM.SecretKey) ? string.Empty : Cryptography.EncryptContent(serviceProviderVM.SecretKey);
                provider.Element("UpdatedDate").Value = serviceProviderVM.UpdatedDate.ToString();
                provider.Element("UpdatedUser").Value = serviceProviderVM.UpdatedUser.ToString();

            }
            blob.UploadText(document.ToString());
        }

        private IEnumerable<SelectListItem> GetServiceProviderNameList()
        {
            return new SelectList(ServiceProviderManager.GetServiceProviderList(), "Id", "Name");
        }

        private IEnumerable<SelectListItem> GetServiceProviderNameList(int id)
        {
            return new SelectList(ServiceProviderManager.GetServiceProviderList(), "Id", "Name", id);
        }
    }
}