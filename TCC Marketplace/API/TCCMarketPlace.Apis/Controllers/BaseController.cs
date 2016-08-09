using System;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using TCCMarketPlace.Business;
using TCCMarketPlace.Apis.Extensions;
using TCCMarketPlace.Model;
using TCCMarketPlace.Model.ExceptionHandlers;

namespace TCCMarketPlace.Apis.Controllers
{
    public abstract class BaseController : ApiController
    {
        private User _currentUser;

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
        public BaseController()
        {

        }

        internal string GetJSonUser(LoginRequest loginReq)
        {
            if (_currentUser == null)
            {
                _currentUser = new User
                {
                    UserId = loginReq.UserId,
                    LocationId = loginReq.LocationId,
                    ThermostatId = loginReq.ThermostatId,
                    UserName = loginReq.UserName,
                    ZipCode = loginReq.ZipCode,
                    UserType = loginReq.UserType ?? "TCC",
                    MacID = loginReq.MacID
                };
            }
            return Base64Encode(JsonConvert.SerializeObject(_currentUser));
        }

        private string Base64Encode(string userData)
        {
            var encryptedUser = Cryptography.Encrypt(userData);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encryptedUser);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            var decodedUser = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            return Cryptography.Decrypt(decodedUser);
        }

        private User GetUserDetails()
        {
            var user = System.Web.HttpContext.Current.User;

            var jsonUser = System.Web.HttpContext.Current.Request.GetHeaderValue("User-Data");

            if (!string.IsNullOrEmpty(jsonUser))
            {
                try
                {
                    var decodedUser = Base64Decode(jsonUser);
                    _currentUser = JsonConvert.DeserializeObject<User>(decodedUser);

                }
                catch
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
                }

            }

            return _currentUser;
        }

        protected void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(a => a.Errors).Select(b => b.ErrorMessage).Where(c => !string.IsNullOrEmpty(c)).ToList();
                throw new BusinessException(errors);
            }
        }
    }
}
