namespace Marketplace.Admin.Data.Infrastructure
{
    /// <summary>
    /// DB factory for Entity framework.
    /// </summary>
    public class DbFactory : Disposable, IDbFactory
    {
        MarketplaceAdminDb dbContext;

        /// <summary>
        /// Initialize dbContext 
        /// </summary>
        /// <returns> MarketplaceAdminDb Context.</returns>
        public MarketplaceAdminDb Init()
        {
            return dbContext ?? (dbContext = new MarketplaceAdminDb());
        }

        /// <summary>
        /// Disposes custom objects.
        /// </summary>
        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
