using System;
using System.Threading.Tasks;
using TCCMarketPlace.Model;

namespace TCCMarketPlace.Business.Interface
{
    public interface IThirdPartyService : IDisposable
    {
        Task<bool> UnEnroll(User user, Service service);
    }
}
