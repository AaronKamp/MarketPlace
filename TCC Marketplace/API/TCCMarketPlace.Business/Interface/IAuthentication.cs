using System;
using System.Threading.Tasks;
using TCCMarketPlace.Model;
using TCCMarketPlace.Model.TccOAuth;

namespace TCCMarketPlace.Business.Interface
{
    /// <summary>
    /// Interface to handle various authentication providers
    /// </summary>
    public interface IAuthentication : IDisposable
    {
        /// <summary>
        /// Validates user against the login credentials
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        Task<LoginResult> ValidateUser(LoginRequest loginRequest);
    }
}
