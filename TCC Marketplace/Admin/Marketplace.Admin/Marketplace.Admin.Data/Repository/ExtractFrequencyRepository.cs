using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to ExtractFrequency repository.
    /// </summary>
    public interface IExtractFrequencyRepository : IRepository<ExtractFrequency>
    {
    }

    /// <summary>
    /// Handles database operations for extract ExtractFrequency entity.
    /// </summary>
    public class ExtractFrequencyRepository : RepositoryBase<ExtractFrequency>, IExtractFrequencyRepository
    {
        public ExtractFrequencyRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
