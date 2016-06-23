using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Marketplace.Admin.Core.DTO;

namespace Marketplace.Admin.Data.Repository
{

    public interface IUserRepository : IRepository<AspNetUser>
    {
        AspNetUser FindByUserName(string userName);
        Task<AspNetUser> FindByUserNameAsync(string username);
        Task<AspNetUser> FindByUserNameAsync(System.Threading.CancellationToken cancellationToken, string username);
        AspNetUser FindByUserEMail(string email);
        UserPaginationDTO GetUsers(int pageSize, int? page);
    }

    public class UserRepository : RepositoryBase<AspNetUser>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

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
        public AspNetUser FindByUserName(string username)
        {
            return DbContext.AspNetUsers.FirstOrDefault(x => x.UserName == username);
        }

        public AspNetUser FindByUserEMail(string email)
        {
            return DbContext.AspNetUsers.FirstOrDefault(x => x.Email == email);
        }

        public Task<AspNetUser> FindByUserNameAsync(string username)
        {
            return DbContext.AspNetUsers.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public Task<AspNetUser> FindByUserNameAsync(System.Threading.CancellationToken cancellationToken, string username)
        {
            return DbContext.AspNetUsers.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }
    }
}
