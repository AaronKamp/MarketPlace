using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;
using System.Linq;
using System.Data.Entity;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to SCFRepository.
    /// </summary>
    public interface ISCFRepository : IRepository<SCF>
    {
    }

    /// <summary>
    /// Handles database operations for SCF entity.
    /// </summary>
    public class SCFRepository : RepositoryBase<SCF>, ISCFRepository
    {
        public SCFRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

        /// <summary>
        /// Gets List of SCF by Expression.
        /// </summary>
        /// <param name="where"> Expression.</param>
        /// <returns> SCF List</returns>
        public override IEnumerable<SCF> GetMany(Expression<Func<SCF, bool>> where)
        {
            return DbContext.SCFs.Include(x => x.State).Include(c => c.State.Country).Where(@where);
        }
    }
}
