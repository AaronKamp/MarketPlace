using Microsoft.AspNet.Identity;

namespace Marketplace.Admin.Identity
{
    public class IdentityRole : IRole<int>
    {
        public IdentityRole()
        {         
        }

        public IdentityRole(string name)
        {
            Name = name;
        }

        public IdentityRole(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
