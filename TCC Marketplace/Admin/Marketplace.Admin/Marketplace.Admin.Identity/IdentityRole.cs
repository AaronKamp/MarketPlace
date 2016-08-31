using Microsoft.AspNet.Identity;

namespace Marketplace.Admin.Identity
{
    /// <summary>
    /// User identity role model.
    /// </summary>
    public class IdentityRole : IRole<int>
    {
        public IdentityRole()
        {         
        }

        /// <summary>
        /// Sets Identity role name.
        /// </summary>
        /// <param name="name"> Role name.</param>
        public IdentityRole(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Sets Identity role object.
        /// </summary>
        /// <param name="name"> Identity Role name.</param>
        /// <param name="id"> Role Id.</param>
        public IdentityRole(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
