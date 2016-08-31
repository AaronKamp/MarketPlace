using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Xml.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Marketplace.Admin.ViewModels;
using Marketplace.Admin.Core;

namespace Marketplace.Admin.Utils
{
    /// <summary>
    /// Handles Service provider functionalities as there is no DB context now.
    /// </summary>
    public static class ServiceProviderManager
    {
        private const string xmlDirectory = "xml";

        /// <summary>
        /// Gets the Cloud Block Blob
        /// </summary>
        /// <returns>CloudBlockBlob</returns>
        public static CloudBlockBlob GetCloudBlockBlob()
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
            var directory = container.GetDirectoryReference(xmlDirectory.ToLower());

            var uniqueBlobName = $"{ConfigurationManager.AppSettings["Application.Environment"].ToLower()}/{xmlDirectory.ToLower()}/serviceprovider_xml.xml";
            return blobStorage.GetBlockBlobReference(uniqueBlobName);
        }

        /// <summary>
        /// Gets the list of active Service providers.
        /// </summary>
        /// <returns> List of active service providers.</returns>
        public static IEnumerable<ServiceProviderViewModel> GetActiveServiceProviderList()
        {
            return GetServiceProviderList().Where(p=>p.IsActive);
        }

        /// <summary>
        /// Gets the list of all service providers.
        /// </summary>
        /// <returns>List of all service providers. </returns>
        public static IEnumerable<ServiceProviderViewModel> GetServiceProviderList()
        {
            var blob = GetCloudBlockBlob();
            var document = GetServiceProviderXml(blob);
            var detailsList = document.Descendants("ServiceProvider")
                                        .Where(p => !Convert.ToBoolean(p.Element("IsDeleted").Value))
                                        .Select(p => new ServiceProviderViewModel
                                        {
                                            Id = Convert.ToInt32(p.Element("Id").Value),
                                            Name = p.Element("Name").Value,
                                            IsActive = Convert.ToBoolean(p.Element("IsActive").Value),
                                            IsDeleted = Convert.ToBoolean(p.Element("IsDeleted").Value),
                                            SignUpUrl = p.Element("SignUpUrl").Value,
                                            StatusUrl = p.Element("StatusUrl").Value,
                                            UnEnrollUrl = p.Element("UnEnrollUrl").Value,
                                            TokenUrl = p.Element("TokenUrl").Value,
                                            GenerateBearerToken = Convert.ToBoolean(p.Element("GenerateBearerToken").Value),
                                            AppId = string.IsNullOrWhiteSpace(p.Element("AppId").Value) ? string.Empty : Cryptography.DecryptContent(p.Element("AppId").Value),
                                            SecretKey = string.IsNullOrWhiteSpace(p.Element("SecretKey").Value) ? string.Empty : Cryptography.DecryptContent(p.Element("SecretKey").Value),
                                            CreatedDate = Convert.ToDateTime(p.Element("CreatedDate").Value),
                                            UpdatedDate = Convert.ToDateTime(p.Element("UpdatedDate").Value),
                                            UpdatedUser = p.Element("UpdatedUser").Value
                                        }).ToList();
            return detailsList;
        }

        /// <summary>
        /// Gets the service provider xml from the blob.
        /// </summary>
        /// <param name="blob">CloudBlockBlob </param>
        /// <returns> XDocument </returns>
        public static XDocument GetServiceProviderXml(CloudBlockBlob blob)
        {
            if (!blob.Exists())
            {
                CreateCloudBlob(blob);
            }
            var xml = blob.DownloadText();
            var xDoc = XDocument.Parse(xml);
            return xDoc;
        }

        /// <summary>
        /// Creates cloud block blob if not exists.
        /// </summary>
        /// <param name="blob"> CloudBlockBlob</param>
        private static void CreateCloudBlob(CloudBlockBlob blob)
        {
            var document = new XDocument();
            document.Add(new XElement("ServiceProviderInfo"));
            blob.Properties.ContentType = "text/xml;charset=utf-8";
            blob.UploadText(document.ToString());
        }

        /// <summary>
        /// Gets the sign-up URL for the service provider by Id.
        /// </summary>
        /// <param name="id"> Service Provider id.</param>
        /// <returns> sign-up URL</returns>
        internal static string GetSignUpUrl(int id)
        {
            var blob = GetCloudBlockBlob();
            var document = GetServiceProviderXml(blob);
            var signUpUrl = document.Descendants("ServiceProvider")
                                            .Where(p => Convert.ToInt32(p.Element("Id").Value) == id)
                                            .Select(p => p.Element("SignUpUrl").Value)
                                            .FirstOrDefault();
            return signUpUrl;

        }
    }
}