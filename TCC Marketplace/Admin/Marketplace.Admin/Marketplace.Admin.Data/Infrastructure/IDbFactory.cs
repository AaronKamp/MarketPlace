using System;

namespace Marketplace.Admin.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        MarketplaceAdminDb Init();
    }
}
