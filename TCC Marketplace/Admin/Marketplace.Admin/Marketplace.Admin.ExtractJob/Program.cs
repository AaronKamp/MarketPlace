using Microsoft.Azure.WebJobs;

namespace Marketplace.Admin.ExtractJob
{
    /// <summary>
    /// Entry class in a Web job.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main function. Job host is created here and the required method in Functions class is called.
        /// </summary>
        /// <remarks>
        /// Please set the following connection strings in app.config for this WebJob to run:
        /// AzureWebJobsDashboard and AzureWebJobsStorage
        /// </remarks>
        static void Main()
        {
            var host = new JobHost();

            // The following code will invoke a function called UploadFilesToFTP and 
            // pass in data (value in this case) to the function
            host.Call(typeof(Functions).GetMethod("UploadFilesToFTP"));
        }
    }
}
