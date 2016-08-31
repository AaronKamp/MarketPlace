using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to SettingsRepository.
    /// </summary>
    public interface ISettingsRepository : IRepository<ConfigurationSettings>
    {
    }

    /// <summary>
    /// Handles database operations for Settings entity.
    /// </summary>
    public class SettingsRepository : RepositoryBase<ConfigurationSettings>, ISettingsRepository
    {
        public SettingsRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
