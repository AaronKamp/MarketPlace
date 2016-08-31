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
    /// <summary>
    /// Handles unused slider image deletion. 
    /// </summary>
    public class PurgeSliderImage
    {
        private const string sliderImageDirectory = "images/slider";
        private readonly ImageQueueRepository imageQueueRepository;

        /// <summary>
        /// Constructor. Initializes imageQueueRepository.
        /// </summary>
        public PurgeSliderImage()
        {
            imageQueueRepository = new ImageQueueRepository(new DbFactory());
        }

        /// <summary>
        /// Select unused slider images and delete the blob.
        /// </summary>
        /// <param name="log"> Exception log. </param>
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
                    log.WriteLine("Deletion failed for image {0} at {1} with exception {2}", blockBlob.Name, DateTime.UtcNow.ToString(), ex.Message);
                }
            }
            UpdateDeletedFlag(deletedBlobList);
        }

        /// <summary>
        /// Gets the list of image URL eligible for deletion.
        /// </summary>
        /// <returns> List of ImagesURL. </returns>
        private List<string> GetBlobUrlList()
        {
            var imageList = GetQueuedImagesList();
            var blobUrlList = imageList.Select(p => p.ImageUrl).ToList();
            return blobUrlList;
        }
        
        /// <summary>
        /// Gets List of queued images from database.
        /// </summary>
        /// <returns> List of ImageQueue. </returns>
        private List<ImageQueue> GetQueuedImagesList()
        {
            return imageQueueRepository.GetAll().Where(p => (!p.IsDeleted && (DateTime.UtcNow - p.ActualDeletedDate).TotalDays >= 14)).ToList();
        }

        /// <summary>
        /// Updates deleted flag to true after deletion. 
        /// </summary>
        /// <param name="deletedBlobList"> </param>
        private void UpdateDeletedFlag(List<string> deletedBlobList)
        {
            imageQueueRepository.UpdateDeletedFlag(deletedBlobList);
        }

        /// <summary>
        /// Connects to and returns CloudBlockContainer.
        /// </summary>
        /// <returns> CloudBlobContainer. </returns>
        private static CloudBlobContainer GetCloudBlockContainer()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["Application.Environment"].ToLower());
            return container;
        }
    }
}
