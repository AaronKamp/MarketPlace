using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to CountryRepository.
    /// </summary>
    public interface ICountryRepository : IRepository<Country>
    {
    }

    /// <summary>
    /// Handles database operations for Country entity.
    /// </summary>
    public class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
