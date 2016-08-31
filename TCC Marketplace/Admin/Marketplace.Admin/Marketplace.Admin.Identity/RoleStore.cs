using System;
using System.Linq;
using System.Threading.Tasks;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;
using Microsoft.AspNet.Identity;

namespace Marketplace.Admin.Identity
{
    /// <summary>
    /// Extends the RoleStore provided with Microsoft.AspNet.Identity
    /// This is extended to override the default primary key type(GUID) with INT
    /// </summary>
    public class RoleStore : IQueryableRoleStore<IdentityRole, int>
    {
        private readonly IRoleRepository _roleRepository;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="roleRepository"></param>
        public RoleStore(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        #region IRoleStore<IdentityRole, Guid> Members
        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task CreateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var r = getRole(role);

            _roleRepository.Add(r);
            return _roleRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task DeleteAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var r = getRole(role);

            _roleRepository.Remove(r);
            return _roleRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Find a role by id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<IdentityRole> FindByIdAsync(int roleId)
        {
            var role = _roleRepository.FindById(roleId);
            return Task.FromResult(getIdentityRole(role));
        }

        /// <summary>
        /// Find a role by name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            var role = _roleRepository.FindByName(roleName);
            return Task.FromResult(getIdentityRole(role));
        }

        /// <summary>
        /// Update a role
        /// </summary>
        /// <param name="role">IdentityRole</param>
        public void UpdateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            var r = getRole(role);
            _roleRepository.Update(r);
        //    return _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        /// </summary>
        public void Dispose()
        {
        }
        #endregion

        #region IQueryableRoleStore<IdentityRole, Guid> Members

        public IQueryable<IdentityRole> Roles
        {
            get
            {
                return _roleRepository
                    .GetAll()
                    .Select(x => getIdentityRole(x))
                    .AsQueryable();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets AspNet role from Identity role.
        /// </summary>
        /// <param name="identityRole"> Identity Role.</param>
        /// <returns> AspNetRole.</returns>
        private AspNetRole getRole(IdentityRole identityRole)
        {
            if (identityRole == null)
                return null;
            return new AspNetRole
            {
                Id = identityRole.Id,
                Name = identityRole.Name
            };
        }

        /// <summary>
        /// Gets Identity roles from AspNetRole.
        /// </summary>
        /// <param name="role">AspNetRole</param>
        /// <returns>IdentityRole</returns>
        private IdentityRole getIdentityRole(AspNetRole role)
        {
            if (role == null)
                return null;
            return new IdentityRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        /// <summary>
        /// Update a role 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task IRoleStore<IdentityRole,int>.UpdateAsync(IdentityRole role)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
