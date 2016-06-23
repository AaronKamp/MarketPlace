using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{

    public interface ICountryRepository : IRepository<Country>
    {
    }

    public class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
