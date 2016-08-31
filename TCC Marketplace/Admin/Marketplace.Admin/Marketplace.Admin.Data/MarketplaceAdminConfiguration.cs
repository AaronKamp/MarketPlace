using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Configuration;

namespace Marketplace.Admin.Data
{
    public class MarketplaceAdminConfiguration : DbConfiguration
    {
        public MarketplaceAdminConfiguration()
        {
            int maxRetryCount, maxDelay;

            // try and get maxRetryCount, maxDelay from Application settings. If not possible default value is taken. 
            maxRetryCount = int.TryParse(ConfigurationManager.AppSettings["SqlConnectionMaxRetry"], out maxRetryCount) ? maxRetryCount : 5;
            maxDelay = int.TryParse(ConfigurationManager.AppSettings["SqlConnectionMaxDelay"], out maxDelay) ? maxDelay : 12;

            // Set up the execution strategy for SQL Database (exponential).
            SetExecutionStrategy(
                    "System.Data.SqlClient", () => new SqlAzureExecutionStrategy(maxRetryCount, TimeSpan.FromSeconds(maxDelay)));
        }
    }
}