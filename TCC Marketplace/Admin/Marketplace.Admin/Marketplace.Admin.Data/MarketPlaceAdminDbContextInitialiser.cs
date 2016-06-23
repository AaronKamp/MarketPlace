using System;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace Marketplace.Admin.Data
{
    public class MarketPlaceAdminDbContextInitialiser : CreateDatabaseIfNotExists<MarketplaceAdminDb>
    {
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
