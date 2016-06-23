namespace Marketplace.Admin.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        MarketplaceAdminDb dbContext;

        public MarketplaceAdminDb Init()
        {
            return dbContext ?? (dbContext = new MarketplaceAdminDb());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
