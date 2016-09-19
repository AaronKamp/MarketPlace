using System.IO;
using Microsoft.Azure.WebJobs;
using System;

namespace Marketplace.Admin.ExtractJob
{
    /// <summary>
    /// The functions class contains the methods that are being directly called by JobHost in the main().
    /// </summary>
    public class Functions
    {
        /// <summary>
        /// This function will be triggered based on the schedule you have set for this WebJob
        /// This function will enqueue a message on an Azure queue called queue
        /// </summary>
        /// <param name="log"></param>
        [NoAutomaticTrigger]
        public static void UploadFilesToFTP(TextWriter log)
        {
            try
            {
                Marketplace.Admin.Extract.Process extractProcess = new Extract.Process();

                log.WriteLine("Marketplace.Admin.Extract.Process.UploadFiles called at {0}", DateTime.UtcNow.ToString());

                extractProcess.UploadFiles();

                log.WriteLine("Marketplace.Admin.Extract.Process.UploadFiles completed at {0}", DateTime.UtcNow.ToString());
            }
            catch (Exception ex)
            {
                log.WriteLine("Marketplace.Admin.Extract.Process.UploadFiles failed with exception {0}", ex.Message);
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }
    
    }
}

