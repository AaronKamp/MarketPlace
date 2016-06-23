using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCMarketPlace.Model;
using TCCMarketPlace.Model.TccOAuth;

namespace TCCMarketPlace.Business.Interface
{
    public interface IAuthentication : IDisposable
    {
        Task<string> GetBearerToken();
        Task<LoginResult> ValidateUser(LoginRequest loginRequest);
    }
}
