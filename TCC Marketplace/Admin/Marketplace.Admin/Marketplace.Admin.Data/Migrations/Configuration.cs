namespace Marketplace.Admin.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MarketplaceAdminDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Marketplace.Admin.Data.MarketplaceAdminDb";
        }
    }
}
