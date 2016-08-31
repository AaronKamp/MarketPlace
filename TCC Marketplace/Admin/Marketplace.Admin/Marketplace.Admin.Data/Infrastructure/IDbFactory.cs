using System;

namespace Marketplace.Admin.Data.Infrastructure
{
    /// <summary>
    /// Interface to DBFactory
    /// </summary>
    public interface IDbFactory : IDisposable
    {
        MarketplaceAdminDb Init();
    }
}
