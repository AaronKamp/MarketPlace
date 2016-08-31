using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Marketplace.Admin.ScheduledJob
{
    /// <summary>
    /// The functions class contains the methods that are being directly called by JobHost in the main().
    /// </summary>
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        [NoAutomaticTrigger]
        public static void RunImageDeletion(TextWriter log)
        {
            try {
                var purgeImage = new PurgeSliderImage(); 
                log.WriteLine("Marketplace.Admin.ScheduledJob.RunImageDeletion called at {0}", DateTime.UtcNow.ToString());

                purgeImage.PurgeImage(log);

                log.WriteLine("Marketplace.Admin.ScheduledJob.RunImageDeletion completed at {0}", DateTime.UtcNow.ToString());
            }
            catch(Exception ex)
            {
                log.WriteLine("Marketplace.Admin.ScheduledJob.RunImageDeletion failed with exception {0} at {1}", ex.Message, DateTime.UtcNow.ToString());
            }
        }
    }
}
