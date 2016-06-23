using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;
using System.Linq;
using System.Data.Entity;

namespace Marketplace.Admin.Data.Repository
{

    public interface ISCFRepository : IRepository<SCF>
    {
    }

    public class SCFRepository : RepositoryBase<SCF>, ISCFRepository
    {
        public SCFRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

        public override IEnumerable<SCF> GetMany(Expression<Func<SCF, bool>> where)
        {
            return DbContext.SCFs.Include(x => x.State).Include(c => c.State.Country).Where(@where);
        }
    }
}
