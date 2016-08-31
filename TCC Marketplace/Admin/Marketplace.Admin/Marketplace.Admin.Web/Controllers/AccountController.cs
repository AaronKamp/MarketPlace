using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Marketplace.Admin.Business;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Marketplace.Admin.Identity;
using Marketplace.Admin.ViewModels;

namespace Marketplace.Admin.Controllers
{
    /// <summary>
    /// Controls logins.
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private SignInManager<IdentityUser, int> _signInManager;
        private readonly IUserManager _userManager;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public AccountController(IUserManager userManager, SignInManager<IdentityUser, int> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Returns login page.
        /// GET: /Account/Login
        /// </summary>
        /// <param name="returnUrl"> Page to be loaded on login. </param>
        /// <returns> Login View.</returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Handles login.
        /// POST: /Account/Login
        /// </summary>
        /// <param name="model"> LoginViewModel </param>
        /// <param name="returnUrl">Page to be loaded on login. </param>
        /// <returns> Return URL View.  </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // validate user credentials
            var user = await _userManager.FindAsync(model.UserName, model.Password);

            //if valid user
            if (user != null)
            {
                //handles login
                await SignInAsync(user, model.RememberMe);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View(model);
        }

        /// <summary>
        /// Handles Login
        /// </summary>
        /// <param name="user">IdentityUser</param>
        /// <param name="isPersistent"></param>
        private async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        public ActionResult Users()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles Sign-out
        /// /Account/LogOff
        /// </summary>
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Services");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        /// <summary>
        /// Get Owin context for current request
        /// </summary>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Handles redirection to URL. 
        /// </summary>
        /// <param name="returnUrl"> Return URL.</param>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Services");
        }

      
        #endregion
    }
}