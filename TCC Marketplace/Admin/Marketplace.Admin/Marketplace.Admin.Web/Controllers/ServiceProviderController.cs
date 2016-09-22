using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web.Mvc;
using Marketplace.Admin.ViewModels;
using Marketplace.Admin.Utils;
using Marketplace.Admin.Core;
using System.Xml.Schema;

namespace Marketplace.Admin.Controllers
{
    /// <summary>
    /// To be reworked with DB implementation in the next phase
    /// Controls service provider manipulation operations.
    /// </summary>
    [Authorize]
    public class ServiceProviderController : Controller
    {

        /// <summary>
        /// Gets the service provider page view.
        /// GET: ServiceProvider.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var list = new Dictionary<int, string>();

            ViewBag.ProviderList = GetServiceProviderNameList();

            return View(new ServiceProviderViewModel());
        }

        /// <summary>
        /// Saves new service provider.
        /// </summary>
        /// <param name="serviceProviderVM"></param>
        /// <returns></returns>
        public ActionResult Save(ServiceProviderViewModel serviceProviderVM)
        {
            if (ModelState.IsValid)
            {
                serviceProviderVM.UpdatedDate = DateTime.UtcNow;
                serviceProviderVM.UpdatedUser = User.Identity.Name;

                //if existing provider, update record
                if (serviceProviderVM.Id > 0)
                {
                    UpdateServiceProvider(serviceProviderVM);
                    TempData["ResponseMessage"] = $"Service provider {serviceProviderVM.Name} has been updated successfully!";
                }
                else
                {
                    //if duplicate, return view with corresponding message
                    if (IsDuplicate(serviceProviderVM.Name))
                    {
                        this.ModelState.AddModelError("Name", "Provider name " + serviceProviderVM.Name + " is duplicate");
                        ViewBag.ProviderList = GetServiceProviderNameList(serviceProviderVM.Id);
                        return View("Index", serviceProviderVM);
                    }
                    else {

                        // else new record, add to database
                        serviceProviderVM.CreatedDate = DateTime.UtcNow;
                        AddServiceProvider(serviceProviderVM);
                        TempData["ResponseMessage"] = $"Service provider {serviceProviderVM.Name} has been saved successfully!";
                    }
                }
                
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProviderList = GetServiceProviderNameList(serviceProviderVM.Id);
                return View("Index", serviceProviderVM);
            }

        }

        /// <summary>
        /// Checks if service provider name is duplicate before adding new service provider.
        /// </summary>
        /// <param name="serviceProviderName"> Entered Service provider name. </param>
        /// <returns>True if the name already exists.</returns>
        private bool IsDuplicate(string serviceProviderName)
        {
          return ServiceProviderManager.GetServiceProviderList()
                .Where(p => p.Name == serviceProviderName).Any();
        }

        /// <summary>
        /// Returns a partial view with service provider details filled in on selecting the required service provider from the dropdown.
        /// </summary>
        /// <param name="id"> Service provider id.</param>
        /// <returns> Partial View.</returns>
        public PartialViewResult Details(int id)
        {
            var serviceProviderViewModel = new ServiceProviderViewModel();

            if (id > 0)
            {
                serviceProviderViewModel = GetServiceProviderDetailsById(id);
            }

            return PartialView("_Edit", serviceProviderViewModel);
        }

        /// <summary>
        /// Gets Service provider details by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ServiceProviderViewModel GetServiceProviderDetailsById(int id)
        {
            return ServiceProviderManager.GetServiceProviderList().Where(p => p.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Handles new service provider addition.
        /// </summary>
        /// <param name="serviceProviderViewModel"> ServiceProviderViewModel </param>
        private void AddServiceProvider(ServiceProviderViewModel serviceProviderViewModel)
        {
            var blob = ServiceProviderManager.GetCloudBlockBlob();
            var document = ServiceProviderManager.GetServiceProviderXml(blob);
            var id = document.Descendants("ServiceProvider").Any() ?
                document.Descendants("ServiceProvider").Select(p => Convert.ToInt32(p.Element("Id").Value)).ToList().Max() + 1 : 1;
            document.Root.Add(new XElement("ServiceProvider",
                                new XElement("Id", id),
                                        new XElement("Name", serviceProviderViewModel.Name),
                                        new XElement("SignUpUrl", serviceProviderViewModel.SignUpUrl),
                                        new XElement("StatusUrl", serviceProviderViewModel.StatusUrl),
                                        new XElement("UnEnrollUrl", serviceProviderViewModel.UnEnrollUrl),
                                new XElement("GenerateBearerToken", serviceProviderViewModel.GenerateBearerToken.ToString().ToLower()),
                                new XElement("IsActive", serviceProviderViewModel.IsActive.ToString().ToLower()),
                                new XElement("IsDeleted", serviceProviderViewModel.IsDeleted.ToString().ToLower()),
                                        new XElement("TokenUrl", serviceProviderViewModel.TokenUrl),
                                new XElement("AppId", string.IsNullOrWhiteSpace(serviceProviderViewModel.AppId) ? string.Empty : Cryptography.EncryptContent(serviceProviderViewModel.AppId)),
                                        new XElement("SecretKey", string.IsNullOrWhiteSpace(serviceProviderViewModel.SecretKey) ? string.Empty : Cryptography.EncryptContent(serviceProviderViewModel.SecretKey)),
                                        new XElement("CreatedDate", serviceProviderViewModel.CreatedDate.ToString()),
                                        new XElement("UpdatedDate", serviceProviderViewModel.UpdatedDate.ToString()),
                                        new XElement("UpdatedUser", serviceProviderViewModel.UpdatedUser)
             ));
            // convert makes valid xml if xml is invalid
            if (ServiceProviderManager.IsXmlInvalid(document))
            {
                document.Descendants("ServiceProvider").ToList().ForEach(a =>
                  {
                      a.Element("GenerateBearerToken").Value = a.Element("GenerateBearerToken").Value.ToLower();
                      a.Element("IsActive").Value = a.Element("IsActive").Value.ToLower();
                      a.Element("IsDeleted").Value = a.Element("IsDeleted").Value.ToLower();
                  });
            }

            try
            {
                //uploads xml if xml valid
                XElement newProvider = document.Descendants("ServiceProvider")
                     .FirstOrDefault(p => Convert.ToInt32(p.Element("Id").Value) == id);
                ServiceProviderManager.IsValidServiceProviderElement(newProvider);

            blob.UploadText(document.ToString());
        }
            catch (XmlSchemaValidationException ex)
            {
                throw new Exception("Service provider validation against schema failed. Could not add new service provider.", ex);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Handles service provider updating.
        /// </summary>
        /// <param name="serviceProviderVM"> ServiceProviderViewModel </param>
        private void UpdateServiceProvider(ServiceProviderViewModel serviceProviderVM)
        {
            var blob = ServiceProviderManager.GetCloudBlockBlob();
            var document = ServiceProviderManager.GetServiceProviderXml(blob);

            if (ServiceProviderManager.IsXmlInvalid(document))
            {
                document.Descendants("ServiceProvider").ToList().ForEach(a =>
                {
                    a.Element("GenerateBearerToken").Value = a.Element("GenerateBearerToken").Value.ToLower();
                    a.Element("IsActive").Value = a.Element("IsActive").Value.ToLower();
                    a.Element("IsDeleted").Value = a.Element("IsDeleted").Value.ToLower();
                });
            }

            XElement provider = document.Descendants("ServiceProvider")
                    .FirstOrDefault(p => Convert.ToInt32(p.Element("Id").Value) == serviceProviderVM.Id);
            if (provider == null)
            {
                throw new KeyNotFoundException("Unable to find a record matching the key");
            }
            else
            {
                provider.Element("Name").Value = serviceProviderVM.Name;
                provider.Element("SignUpUrl").Value = serviceProviderVM.SignUpUrl;
                provider.Element("StatusUrl").Value = serviceProviderVM.StatusUrl ?? string.Empty;
                provider.Element("UnEnrollUrl").Value = serviceProviderVM.UnEnrollUrl ?? string.Empty;
                provider.Element("GenerateBearerToken").Value = serviceProviderVM.GenerateBearerToken.ToString().ToLower();
                provider.Element("IsActive").Value = serviceProviderVM.IsActive.ToString().ToLower();
                provider.Element("IsDeleted").Value = serviceProviderVM.IsDeleted.ToString().ToLower();
                provider.Element("TokenUrl").Value = serviceProviderVM.TokenUrl ?? string.Empty;
                provider.Element("AppId").Value = string.IsNullOrWhiteSpace(serviceProviderVM.AppId) ? string.Empty : Cryptography.EncryptContent(serviceProviderVM.AppId);
                provider.Element("SecretKey").Value = string.IsNullOrWhiteSpace(serviceProviderVM.SecretKey) ? string.Empty : Cryptography.EncryptContent(serviceProviderVM.SecretKey);
                provider.Element("UpdatedDate").Value = serviceProviderVM.UpdatedDate.ToString();
                provider.Element("UpdatedUser").Value = serviceProviderVM.UpdatedUser.ToString();

            }
            try
            {
                ServiceProviderManager.IsValidServiceProviderElement(provider);
            blob.UploadText(document.ToString());
        }
            catch (XmlSchemaValidationException ex)
            {
                throw new Exception("Service provider validation against schema failed. Could not add new service provider.", ex);
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Returns service provider drop down list.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SelectListItem> GetServiceProviderNameList()
        {
            return new SelectList(ServiceProviderManager.GetServiceProviderList(), "Id", "Name");
        }

        /// <summary>
        /// Returns service provider drop down list with one item selected.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerable<SelectListItem> GetServiceProviderNameList(int id)
        {
            return new SelectList(ServiceProviderManager.GetServiceProviderList(), "Id", "Name", id);
        }
    }
}