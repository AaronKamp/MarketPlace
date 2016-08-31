namespace Marketplace.Admin.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory _dbFactory;
        private MarketplaceAdminDb _dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// Database Context object for Entity framework.
        /// </summary>
        public MarketplaceAdminDb DbContext
        {
            get { return _dbContext ?? (_dbContext = _dbFactory.Init()); }
        }

        /// <summary>
        /// Save database changes.
        /// </summary>
        public void Commit()
        {
            DbContext.Commit();
        }
    }
}
