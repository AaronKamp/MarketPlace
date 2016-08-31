using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Marketplace.Admin.Core.DTO;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Identity;
using Marketplace.Admin.Model;
using Microsoft.AspNet.Identity;

namespace Marketplace.Admin.Business
{
    /// <summary>
    /// Business class that handles user functionalities
    /// </summary>
   public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser, int> _userManager;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="userManager"></param>
        public UserManager(IUserRepository userRepository, IUnitOfWork unitOfWork, UserManager<IdentityUser, int> userManager)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns> List of AspNetUser.</returns>
        public IList<AspNetUser> GetUsers()
        {
            return _userRepository.GetAll().OrderBy(u => u.UserName).ToList();
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> AspNetUser </returns>
        public AspNetUser GetUser(int userId)
        {
            return _userRepository.GetById(userId);
        }

        /// <summary>
        /// Update user details.
        /// </summary>
        /// <param name="user"> AspNetUser</param>
        public void UpdateUser(AspNetUser user)
        {
            var existing = _userRepository.GetById(user.Id);
            _userRepository.Update(existing);
        }

        /// <summary>
        /// Saves user changes
        /// </summary>
        public void SaveUser()
        {
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Gets user list for specific page no.
        /// </summary>
        /// <param name="pageSize">No. of users per page. </param>
        /// <param name="page"> Page no.</param>
        /// <returns> user list.</returns>
        public UserPaginationDTO GetUsers(int pageSize, int? page)
        {
            return _userRepository.GetUsers(pageSize, page);
        }

        /// <summary>
        /// Creates new user.
        /// </summary>
        /// <param name="user">Identity user. </param>
        /// <param name="password"> Password.</param>
        /// <returns> Identity result. </returns>
        public IdentityResult Create(IdentityUser user, string password)
        {
            return _userManager.Create(user, password);
        }

        /// <summary>
        /// Updates existing user.
        /// </summary>
        /// <param name="user"> Identity user.</param>
        /// <param name="password"> Password.</param>
        /// <returns> Identity result.</returns>
        public IdentityResult Update(IdentityUser user, string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(password);
            }
            return _userManager.Update(user);
        }

        /// <summary>
        ///  Return a user with the specified username and password or null if there is no match. Typically used for logging in.
        /// </summary>
        /// <param name="userName"> UserName</param>
        /// <param name="password"> Password.</param>
        /// <returns> Identity user.</returns>
        public async Task<IdentityUser> FindAsync(string userName, string password)
        {
            
            var user = await _userManager.FindAsync(userName, password);
            return user;
        }

        /// <summary>
        /// Creates ClaimsIdentity representing the user.
        /// </summary>
        /// <param name="user"> Identity User.</param>
        /// <param name="applicationCookie"> Application cookie.</param>
        /// <returns> Claims identity.</returns>
        public async Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string applicationCookie)
        {
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return identity;
        }
    }
}
