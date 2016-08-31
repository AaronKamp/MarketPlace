using System;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to RoleRepository.
    /// </summary>
    public interface IRoleRepository : IRepository<AspNetRole>
    {
        AspNetRole FindByName(string roleName);
    }

    /// <summary>
    /// Handles database operations for Role entity.
    /// </summary>
    public class RoleRepository : RepositoryBase<AspNetRole>, IRoleRepository
    {
        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="dbFactory"></param>
        public RoleRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

        /// <summary>
        /// Finds role by name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>AspNetRole</returns>
        public AspNetRole FindByName(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
