using System;
using System.Linq;
using System.Web.Http;
using TCCMarketPlace.Model;
using System.Security.Claims;
using System.Threading;

namespace TCCMarketPlace.Apis.Controllers
{
    /// <summary>
    /// Base class for all controllers.
    /// </summary>
    public abstract class BaseController : ApiController
    {
        private User _currentUser;
       
        /// <summary>
        /// User information
        /// </summary>
        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = GetUserDetails();
                }
                return _currentUser;
            }
            set
            {
                value = _currentUser;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseController()
        {

        }

        /// <summary>
        /// Gets user details.
        /// </summary>
        /// <exception cref="HttpResponseException"></exception>
        private User GetUserDetails()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            if (identity == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
            }

            try
            {
                _currentUser = new User();

                _currentUser.UserId = Convert.ToInt32(identity.Claims.Where(c => c.Type == ClaimTypes.UserData)
                                                        .Select(c => c.Value).SingleOrDefault());

                _currentUser.UserName = identity.Claims.Where(c => c.Type == ClaimTypes.Email)
                                                       .Select(c => c.Value).SingleOrDefault();

                _currentUser.UserType = identity.Claims.Where(c => c.Type == ClaimTypes.Role)
                                                       .Select(c => c.Value).SingleOrDefault();

                _currentUser.ZipCode = identity.Claims.Where(c => c.Type == ClaimTypes.PostalCode)
                                                      .Select(c => c.Value).SingleOrDefault();

                _currentUser.LocationId = Convert.ToInt32(identity.Claims.Where(c => c.Type == "locationId")
                                                     .Select(c => c.Value).SingleOrDefault());

                _currentUser.ThermostatId = Convert.ToInt32(identity.Claims.Where(c => c.Type == "thermostatId")
                                                  .Select(c => c.Value).SingleOrDefault());

                _currentUser.MacID = identity.Claims.Where(c => c.Type == "macID")
                                                    .Select(c => c.Value).SingleOrDefault();

            }
            catch
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
            }


            return _currentUser;
        }


    }
}
