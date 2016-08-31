using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Admin.Model;
using Marketplace.Admin.Data.Infrastructure;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to ImageQueueRepository.
    /// </summary>
    public interface IImageQueueRepository : IRepository<ImageQueue>
    {
    }

    /// <summary>
    /// Handles database operations for ImageQueue entity.
    /// </summary>
    public class ImageQueueRepository : RepositoryBase<ImageQueue>, IImageQueueRepository
    {
        public ImageQueueRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

        /// <summary>
        /// Updates deleted flag for deleted blobs.
        /// </summary>
        /// <param name="deletedBlobList"> deleted image blob list</param>
        public void UpdateDeletedFlag(List<string> deletedBlobList)
        {
            var deletedImages = DbContext.ScheduledItems
                                    .Where(p => deletedBlobList.Contains(p.ImageUrl)).ToList();
            deletedImages.ForEach(img => 
                            {
                                img.IsDeleted = true;
                                img.PurgedDate = DateTime.UtcNow;
                            });
            DbContext.SaveChanges();
        }
    }
}
