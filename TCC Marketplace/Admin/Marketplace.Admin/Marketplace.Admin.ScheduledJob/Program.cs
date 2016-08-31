using Microsoft.Azure.WebJobs;

namespace Marketplace.Admin.ScheduledJob
{
    /// <summary>
    /// WebJob entry class.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Please set the following connection strings in app.config for this WebJob to run:
        /// AzureWebJobsDashboard and AzureWebJobsStorage
        /// </summary>
        static void Main()
        {
            var host = new JobHost();

            // The following code will invoke a function called RunImageDeletion and 
            // pass in data (value in this case) to the function.
            host.Call(typeof(Functions).GetMethod("RunImageDeletion"));
        }
    }
}
