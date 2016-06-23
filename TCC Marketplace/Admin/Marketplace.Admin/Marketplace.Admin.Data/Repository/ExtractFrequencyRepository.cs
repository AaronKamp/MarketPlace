using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{

    public interface IExtractFrequencyRepository : IRepository<ExtractFrequency>
    {
    }

    public class ExtractFrequencyRepository : RepositoryBase<ExtractFrequency>, IExtractFrequencyRepository
    {
        public ExtractFrequencyRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
