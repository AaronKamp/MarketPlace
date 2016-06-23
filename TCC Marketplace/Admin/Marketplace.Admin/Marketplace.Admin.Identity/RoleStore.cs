using System;
using System.Linq;
using System.Threading.Tasks;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;
using Microsoft.AspNet.Identity;

namespace Marketplace.Admin.Identity
{
    public class RoleStore : IQueryableRoleStore<IdentityRole, int>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleStore(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        #region IRoleStore<IdentityRole, Guid> Members
        public Task CreateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var r = getRole(role);

            _roleRepository.Add(r);
            return _roleRepository.SaveChangesAsync();
        }

        public Task DeleteAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var r = getRole(role);

            _roleRepository.Remove(r);
            return _roleRepository.SaveChangesAsync();
        }

        public Task<IdentityRole> FindByIdAsync(int roleId)
        {
            var role = _roleRepository.FindById(roleId);
            return Task.FromResult(getIdentityRole(role));
        }

        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            var role = _roleRepository.FindByName(roleName);
            return Task.FromResult(getIdentityRole(role));
        }

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
        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
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

        Task IRoleStore<IdentityRole,int>.UpdateAsync(IdentityRole role)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
