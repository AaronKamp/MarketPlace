using System;
using System.Threading.Tasks;
using TCCMarketPlace.Model;

namespace TCCMarketPlace.Business.Interface
{
    public interface IThirdPartyService : IDisposable
    {
        /// <summary>
        /// Handles delist subscription from third party service providers
        /// </summary>
        /// <param name="user"></param>
        /// <param name="service"></param>
        Task<bool> UnEnroll(User user, Service service);
    }
}
