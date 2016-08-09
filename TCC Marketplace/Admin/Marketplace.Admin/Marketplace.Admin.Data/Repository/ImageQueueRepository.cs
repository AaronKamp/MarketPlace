using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Admin.Model;
using Marketplace.Admin.Data.Infrastructure;

namespace Marketplace.Admin.Data.Repository
{
    public interface IImageQueueRepository : IRepository<ImageQueue>
    {
    }
    public class ImageQueueRepository : RepositoryBase<ImageQueue>, IImageQueueRepository
    {
        public ImageQueueRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

        public void UpdateDeletedFlag(List<string> deletedBlobList)
        {
            var deletedImages = DbContext.ScheduledItems
                                    .Where(p => deletedBlobList.Contains(p.ImageUrl)).ToList();
            deletedImages.ForEach(img => 
                            {
                                img.IsDeleted = true;
                                img.PurgedDate = DateTime.Now;
                            });
            DbContext.SaveChanges();
        }
    }
}
