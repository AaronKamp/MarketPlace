using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;
using TCCMarketPlace.Model;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace TCCMarketPlace.Business
{
    public class ServiceProviderManager
    {
        private const string xmlDirectory = "xml";
        public static IEnumerable<ServiceProvider> GetServiceProviderList()
        {
            var blob = GetCloudBlockBlob();
            var document = GetServiceProviderXml(blob);
            var detailsList = document.Descendants("ServiceProvider")
                                        .Where(p=> !Convert.ToBoolean(p.Element("IsDeleted").Value))
                                        .Select(p => new ServiceProvider
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
                                            AppId = string.IsNullOrWhiteSpace(p.Element("AppId").Value) ? string.Empty : Cryptography.Decrypt(p.Element("AppId").Value),
                                            SecretKey = string.IsNullOrWhiteSpace(p.Element("SecretKey").Value) ? string.Empty: Cryptography.Decrypt(p.Element("SecretKey").Value),
                                            CreatedDate = Convert.ToDateTime(p.Element("CreatedDate").Value),
                                            UpdatedDate = Convert.ToDateTime(p.Element("UpdatedDate").Value),
                                            UpdatedUser = p.Element("UpdatedUser").Value
                                        }).ToList();
            return detailsList;
        }

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
            var uniqueBlobName = $"{ConfigurationManager.AppSettings["Application.Environment"].ToLower()}/{xmlDirectory.ToLower()}/serviceprovider_xml.xml";
            return blobStorage.GetBlockBlobReference(uniqueBlobName);
        }

        public static XDocument GetServiceProviderXml(CloudBlockBlob blob)
        {
            var xml = blob.DownloadText();
            var xDoc = XDocument.Parse(xml);
            return xDoc;
        }
    }
}
