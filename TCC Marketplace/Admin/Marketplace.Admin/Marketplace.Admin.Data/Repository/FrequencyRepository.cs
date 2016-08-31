using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to FrequencyRepository.
    /// </summary>
    public interface IFrequencyRepository : IRepository<Frequency>
    {
    }

    /// <summary>
    /// Handles database operations for Frequency entity.
    /// </summary>
    public class FrequencyRepository : RepositoryBase<Frequency>, IFrequencyRepository
    {
        public FrequencyRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
