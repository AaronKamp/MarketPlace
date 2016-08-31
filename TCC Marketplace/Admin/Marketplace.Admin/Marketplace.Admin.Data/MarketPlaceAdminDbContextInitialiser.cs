using System;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace Marketplace.Admin.Data
{
    /// <summary>
    /// Handles seed data on completion of Database model creation by code first.
    /// </summary>
    public class MarketPlaceAdminDbContextInitialiser : CreateDatabaseIfNotExists<MarketplaceAdminDb>
    {
        /// <summary>
        /// Handles seed data. Reads SQL scripts from file and execute on DB.
        /// </summary>
        /// <param name="context"> MarketplaceAdminDb </param>
        protected override void Seed(MarketplaceAdminDb context)
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var postDeploymentScriptsDirectory  = appDirectory + "bin\\PostDeploymentScripts";
            var sqlfiles = Directory.GetFiles(postDeploymentScriptsDirectory, "*sql").OrderBy(Name => Name);
            foreach(string file in sqlfiles)
            {
                context.Database.ExecuteSqlCommand(File.ReadAllText(file));
            }
            base.Seed(context);
        }
    }
}
