using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Admin.Core.DTO;
using Marketplace.Admin.Identity;
using Marketplace.Admin.Model;
using Microsoft.AspNet.Identity;

namespace Marketplace.Admin.Business
{
     public interface IUserManager
    {
        IList<AspNetUser> GetUsers();
        AspNetUser GetUser(int userId);
        void UpdateUser(AspNetUser user);
        void SaveUser();
        UserPaginationDTO GetUsers(int pageSize, int? page);
        IdentityResult Create(IdentityUser user, string password);
        IdentityResult Update(IdentityUser user, string password);
        Task<IdentityUser> FindAsync(string userName, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string applicationCookie);
    }
}
