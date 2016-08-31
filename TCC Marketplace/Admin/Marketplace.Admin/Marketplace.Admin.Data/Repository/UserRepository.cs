using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Marketplace.Admin.Core.DTO;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to UserRepository.
    /// </summary>
    public interface IUserRepository : IRepository<AspNetUser>
    {
        /// <summary>
        /// Finds user by Username
        /// </summary>
        AspNetUser FindByUserName(string userName);

        /// <summary>
        /// Find user by username asynchronously.
        /// </summary>
        Task<AspNetUser> FindByUserNameAsync(string username);

        /// <summary>
        /// Find user by username asynchronously with cancellation token.
        /// </summary>
        Task<AspNetUser> FindByUserNameAsync(System.Threading.CancellationToken cancellationToken, string username);

        /// <summary>
        /// Finds User by email.
        /// </summary>
        AspNetUser FindByUserEMail(string email);

        /// <summary>
        /// Gets User list by page.
        /// </summary>
        UserPaginationDTO GetUsers(int pageSize, int? page);
    }

    /// <summary>
    /// Handles database operations for User entity.
    /// </summary>
    public class UserRepository : RepositoryBase<AspNetUser>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

        /// <summary>
        /// Gets User list by page.
        /// </summary>
        /// <param name="pageSize"> No. of records in a page.</param>
        /// <param name="page"> Page no.</param>
        /// <returns> List of Users</returns>
        public UserPaginationDTO GetUsers(int pageSize, int? page)
        {
            int pageNo = page == null ? 1 : (int)page;

            UserPaginationDTO userPageDto = new UserPaginationDTO();
            userPageDto.PageSize = pageSize;
            userPageDto.CurrentPage = pageNo;

            var users = DbContext.AspNetUsers;
            userPageDto.TotalRecord = users.Count();

            userPageDto.NoOfPages = (userPageDto.TotalRecord / userPageDto.PageSize) + ((userPageDto.TotalRecord % userPageDto.PageSize) > 0 ? 1 : 0);

            userPageDto.CurrentPage = pageNo;

            userPageDto.Users = users
                .Select(x => new UserListDTO
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    Email = x.Email
                }).OrderBy(x => x.UserName).Skip((pageNo - 1) * userPageDto.PageSize).Take(userPageDto.PageSize).ToList();

            return userPageDto;
        }

        /// <summary>
        /// Finds user by Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns> User Entity</returns>
        public AspNetUser FindByUserName(string username)
        {
            return DbContext.AspNetUsers.FirstOrDefault(x => x.UserName == username);
        }

        /// <summary>
        /// Finds User by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns> user Entity</returns>
        public AspNetUser FindByUserEMail(string email)
        {
            return DbContext.AspNetUsers.FirstOrDefault(x => x.Email == email);
        }

        /// <summary>
        /// Find user by username asynchronously.
        /// </summary>
        /// <param name="username"></param>
        /// <returns> Entity</returns>
        public Task<AspNetUser> FindByUserNameAsync(string username)
        {
            return DbContext.AspNetUsers.FirstOrDefaultAsync(x => x.UserName == username);
        }

        /// <summary>
        /// Find user by username asynchronously with cancellation token.
        /// </summary>
        /// <param name="username"></param>
        /// <returns> Entity</returns>
        public Task<AspNetUser> FindByUserNameAsync(System.Threading.CancellationToken cancellationToken, string username)
        {
            return DbContext.AspNetUsers.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }
    }
}
