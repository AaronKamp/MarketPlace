using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.ScheduledJob
{
    public class PurgeSliderImage
    {
        private const string sliderImageDirectory = "images/slider";
        private readonly ImageQueueRepository imageQueueRepository;

        public PurgeSliderImage()
        {
            imageQueueRepository = new ImageQueueRepository(new DbFactory());
        }
        public void PurgeImage(TextWriter log)
        {
            var blobUrlList = GetBlobUrlList();
            var deletedBlobList = new List<string>();
            var container = GetCloudBlockContainer();
            CloudBlockBlob blockBlob;
            foreach (var blobUrl in blobUrlList)
            {
                var blobDir = container.GetDirectoryReference($"{sliderImageDirectory.ToLower()}");
                 blockBlob= blobDir.GetBlockBlobReference(blobUrl.Split('/').LastOrDefault());

                try
                {
                    blockBlob.Delete();
                    deletedBlobList.Add(blobUrl);
                }
                catch (Exception ex)
                {
                    log.WriteLine("Deletion failed for image {0} at {1} with exception {2}", blockBlob.Name, DateTime.Now.ToString(), ex.Message);
                }
            }

            UpdateDeletedFlag(deletedBlobList);
        }

        private List<string> GetBlobUrlList()
        {
            var imageList = GetQueuedImagesList();
            var blobUrlList = imageList.Select(p => p.ImageUrl).ToList();
            return blobUrlList;
        }
         
        private List<ImageQueue> GetQueuedImagesList()
        {
            return imageQueueRepository.GetAll().Where(p => (!p.IsDeleted && (DateTime.Now - p.ActualDeletedDate).TotalDays >= 14)).ToList();
        }
        private void UpdateDeletedFlag(List<string> deletedBlobList)
        {
            imageQueueRepository.UpdateDeletedFlag(deletedBlobList);
        }

        private static CloudBlobContainer GetCloudBlockContainer()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["Application.Environment"].ToLower());
            return container;
        }
    }
}
