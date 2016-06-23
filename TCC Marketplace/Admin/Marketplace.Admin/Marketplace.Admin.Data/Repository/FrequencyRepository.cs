using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{

    public interface IFrequencyRepository : IRepository<Frequency>
    {
    }

    public class FrequencyRepository : RepositoryBase<Frequency>, IFrequencyRepository
    {
        public FrequencyRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
