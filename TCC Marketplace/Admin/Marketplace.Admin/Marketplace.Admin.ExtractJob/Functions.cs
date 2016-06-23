using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Data;
using System;
using System.Xml.Linq;

namespace Marketplace.Admin.ExtractJob
{
    public class Functions
    {
        // This function will be triggered based on the schedule you have set for this WebJob
        // This function will enqueue a message on an Azure Queue called queue
        [NoAutomaticTrigger]
        public static void UploadFilesToFTP(TextWriter log)
        {
            try
            {
                Marketplace.Admin.Extract.Process extractProcess = new Extract.Process();

                log.WriteLine("Marketplace.Admin.Extract.Process.UploadFiles called at {0}", DateTime.Now.ToString());

                extractProcess.UploadFiles();

                log.WriteLine("Marketplace.Admin.Extract.Process.UploadFiles completed at {0}", DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                log.WriteLine("Marketplace.Admin.Extract.Process.UploadFiles failed with exception {0}", ex.Message);
            }
        }
    
    }
}

