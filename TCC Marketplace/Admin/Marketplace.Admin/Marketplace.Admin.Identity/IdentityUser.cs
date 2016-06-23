using System;
using Microsoft.AspNet.Identity;

namespace Marketplace.Admin.Identity
{
    public class IdentityUser : IUser<int>
    {
        public IdentityUser()
        {
        }

        public IdentityUser(string userName)
        {
            UserName = userName;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CreatedUser{get;set;}
        public string UpdatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
    }
}
