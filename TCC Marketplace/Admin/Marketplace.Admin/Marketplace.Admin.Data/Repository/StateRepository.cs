using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{

    public interface IStateRepository : IRepository<State>
    {
    }

    public class StateRepository : RepositoryBase<State>, IStateRepository
    {
        public StateRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
