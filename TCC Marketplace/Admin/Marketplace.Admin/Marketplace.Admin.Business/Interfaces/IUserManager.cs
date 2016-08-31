using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Marketplace.Admin.Core.DTO;
using Marketplace.Admin.Identity;
using Marketplace.Admin.Model;
using Microsoft.AspNet.Identity;

namespace Marketplace.Admin.Business
{
    public interface IUserManager
    {
        /// <summary>
        /// Gets all users
        /// </summary>
        IList<AspNetUser> GetUsers();

        /// <summary>
        /// Get user by id.
        /// </summary>
        AspNetUser GetUser(int userId);

        /// <summary>
        /// Update user details.
        /// </summary>
        void UpdateUser(AspNetUser user);

        /// <summary>
        /// Saves user changes
        /// </summary>
        void SaveUser();

        /// <summary>
        /// Gets user list for specific page no.
        /// </summary>
        UserPaginationDTO GetUsers(int pageSize, int? page);

        /// <summary>
        /// Creates new user.
        /// </summary>
        IdentityResult Create(IdentityUser user, string password);

        /// <summary>
        /// Updates existing user.
        /// </summary>
        IdentityResult Update(IdentityUser user, string password);

        /// <summary>
        ///  Return a user with the specified username and password or null if there is no match. Typically used for logging in.
        /// </summary>
        Task<IdentityUser> FindAsync(string userName, string password);

        /// <summary>
        /// Creates ClaimsIdentity representing the user.
        /// </summary>
        Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string applicationCookie);
    }
}
