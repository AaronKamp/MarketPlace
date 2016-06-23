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
   public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser, int> _userManager;

        public UserManager(IUserRepository userRepository, IUnitOfWork unitOfWork, UserManager<IdentityUser, int> userManager)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public AspNetUser GetUser(int userId)
        {
            return _userRepository.GetById(userId);
        }

        public IList<AspNetUser> GetUsers()
        {
            return _userRepository.GetAll().OrderBy(u => u.UserName).ToList();
        }

        public void UpdateUser(AspNetUser user)
        {
            var existing = _userRepository.GetById(user.Id);
            _userRepository.Update(existing);
        }

        public void SaveUser()
        {
            _unitOfWork.Commit();
        }

        public UserPaginationDTO GetUsers(int pageSize, int? page)
        {
            return _userRepository.GetUsers(pageSize, page);
        }

        public IdentityResult Create(IdentityUser user, string password)
        {
            return _userManager.Create(user, password);
        }

        public IdentityResult Update(IdentityUser user, string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(password);
            }
            return _userManager.Update(user);
        }

        public async Task<IdentityUser> FindAsync(string userName, string password)
        {
            
            var user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string applicationCookie)
        {
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return identity;
        }
    }
}
