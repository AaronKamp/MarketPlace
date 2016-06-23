using System;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{

    public interface IRoleRepository : IRepository<AspNetRole>
    {
        AspNetRole FindByName(string roleName);
    }

    public class RoleRepository : RepositoryBase<AspNetRole>, IRoleRepository
    {
        public RoleRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

        public AspNetRole FindByName(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
