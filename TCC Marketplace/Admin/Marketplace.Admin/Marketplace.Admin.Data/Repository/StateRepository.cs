using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to StateRepository.
    /// </summary>
    public interface IStateRepository : IRepository<State>
    {
    }

    /// <summary>
    /// Handles database operations for State entity.
    /// </summary>
    public class StateRepository : RepositoryBase<State>, IStateRepository
    {
        public StateRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
