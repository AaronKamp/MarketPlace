using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Identity
{
    /// <summary>
    /// Extends the UserStore provided with Microsoft.AspNet.Identity
    /// This is extended to override the default primary key type(GUID) with INT
    /// </summary>
    public class UserStore : IUserLoginStore<IdentityUser, int>, IUserClaimStore<IdentityUser, int>, IUserRoleStore<IdentityUser, int>, IUserPasswordStore<IdentityUser, int>, IUserSecurityStampStore<IdentityUser, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="roleRepository"></param>
        public UserStore(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Creates a new IdentityUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateUser(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = GetUser(user);

            _userRepository.Add(u);

            return _userRepository.SaveChangesAsync();
        }

        #region IUserStore<IdentityUser, Guid> Members

        /// <summary>
        ///  Insert a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = GetUser(user);

            _userRepository.Add(u);
            return _userRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = GetUser(user);

            _userRepository.Remove(u);
            return _userRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Finds a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<IdentityUser> FindByIdAsync(int userId)
        {
            var user = _userRepository.FindById(userId);
            return Task.FromResult(GetIdentityUser(user));
        }

        /// <summary>
        /// Find a user by name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            var user = _userRepository.FindByUserName(userName);
            return Task.FromResult(GetIdentityUser(user));

        }

        /// <summary>
        ///   Update a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task UpdateAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentException("user");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            populateUser(u, user);

            _userRepository.Update(u);
            return _userRepository.SaveChangesAsync();
        }


        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }
        #endregion

        #region IUserClaimStore<IdentityUser, Guid> Members

        /// <summary>
        /// Add a new user claim
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var c = new AspNetUserClaim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                AspNetUser = u
            };
            u.AspNetUserClaims.Add(c);

            _userRepository.Update(u);
            return _userRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Returns the claims for the user with the issuer set
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult<IList<Claim>>(u.AspNetUserClaims.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList());
        }

        /// <summary>
        ///  Remove a user claim
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var c = u.AspNetUserClaims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            u.AspNetUserClaims.Remove(c);

            _userRepository.Update(u);
            return _userRepository.SaveChangesAsync();
        }
        #endregion

        #region IUserLoginStore<IdentityUser, Guid> Members
        /// <summary>
        /// Adds a user login with the specified provider and key
        /// </summary>
        /// <param name="user"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var l = new AspNetUserLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                AspNetUser = u
            };
            u.AspNetUserLogins.Add(l);

            _userRepository.Update(u);
            return _userRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Returns the user associated with this login
        /// </summary>
        /// <param name="login"></param>
        /// <returns>IdentityUser</returns>
        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            // This is now handled by UserManager
            throw new NotImplementedException();          
        }

        /// <summary>
        /// Returns the linked accounts for this user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>UserLoginInfo</returns>
        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult<IList<UserLoginInfo>>(u.AspNetUserLogins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList());
        }

        /// <summary>
        /// Removes the user login with the specified combination if it exists
        /// </summary>
        /// <param name="user"></param>
        /// <param name="login"></param>
        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var l = u.AspNetUserLogins.FirstOrDefault(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            u.AspNetUserLogins.Remove(l);

            _userRepository.Update(u);
            return _userRepository.SaveChangesAsync();
        }
        #endregion

        #region IUserRoleStore<IdentityUser, Guid> Members
        /// <summary>
        /// Adds a user to a role
        /// </summary>
        /// <param name="user">IdentityUser</param>
        /// <param name="roleName"></param>
        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName.");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));
            var r = _roleRepository.FindByName(roleName);
            if (r == null)
                throw new ArgumentException("roleName does not correspond to a Role entity.", nameof(roleName));

            u.AspNetRoles.Add(r);
            _userRepository.Update(u);

            return _userRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Returns the roles for this user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult<IList<string>>(u.AspNetRoles.Select(x => x.Name).ToList());
        }

        /// <summary>
        /// Returns true if a user is in the role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult(u.AspNetRoles.Any(x => x.Name == roleName));
        }

        /// <summary>
        /// Removes the role for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _userRepository.GetById(Convert.ToInt32(user.Id));
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var r = u.AspNetRoles.FirstOrDefault(x => x.Name == roleName);
            u.AspNetRoles.Remove(r);

            _userRepository.Update(u);
            return _userRepository.SaveChangesAsync();
        }
        #endregion

        #region IUserPasswordStore<IdentityUser, Guid> Members
        /// <summary>
        /// Get the user password hash
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        /// Returns true if a user has a password set
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        /// <summary>
        ///  Set the user password hash
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserSecurityStampStore<IdentityUser, Guid> Members
        /// <summary>
        /// Get the user security stamp
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.SecurityStamp);
        }

        /// <summary>
        /// Set the security stamp for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates AspNetUser from IdentityUser
        /// </summary>
        /// <param name="identityUser"></param>
        /// <returns></returns>
        private AspNetUser GetUser(IdentityUser identityUser)
        {
            if (identityUser == null)
                return null;

            var user = new AspNetUser();
            populateUser(user, identityUser);

            return user;
        }

        /// <summary>
        /// Maps IdentityUser properties to AspNetUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="identityUser"></param>
        private void populateUser(AspNetUser user, IdentityUser identityUser)
        {
            user.Id = identityUser.Id;
            user.UserName = identityUser.UserName ?? user.UserName;
            user.Email = identityUser.Email ?? user.Email;
            user.CreatedDate = identityUser.Id > 0 ? user.CreatedDate : identityUser.CreatedDate;
            user.CreatedUser = identityUser.CreatedUser ?? user.CreatedUser;
            user.UpdatedDate = identityUser.UpdatedDate;
            user.UpdatedUser = identityUser.UpdatedUser?? user.UpdatedUser;
            user.PasswordHash = identityUser.PasswordHash ?? user.PasswordHash;
            user.SecurityStamp = identityUser.SecurityStamp ?? user.SecurityStamp;
        }

        /// <summary>
        /// Creates Identity user from AspNetUser 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private IdentityUser GetIdentityUser(AspNetUser user)
        {
            if (user == null)
                return null;

            var identityUser = new IdentityUser();
            PopulateIdentityUser(identityUser, user);

            return identityUser;
        }

        /// <summary>
        /// Maps AspNetUser user properties to Identity User.
        /// </summary>
        /// <param name="identityUser"></param>
        /// <param name="user"></param>
        private void PopulateIdentityUser(IdentityUser identityUser, AspNetUser user)
        {
            identityUser.Id = user.Id;
            identityUser.UserName = user.UserName ?? identityUser.UserName;
            identityUser.Email = user.Email ?? identityUser.Email;
            identityUser.PasswordHash = user.PasswordHash ?? identityUser.PasswordHash;
            identityUser.SecurityStamp = user.SecurityStamp ?? identityUser.SecurityStamp;
            
        }
        #endregion
    }
}
