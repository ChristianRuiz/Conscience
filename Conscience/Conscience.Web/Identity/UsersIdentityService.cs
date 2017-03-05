using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Conscience.Web.Models;
using Conscience.DataAccess;
using Conscience.Application.Services;
using System.Net;
using Conscience.Web.Identity;

namespace Conscience.Web.Identity
{
    public class UsersIdentityService : IUsersIdentityService
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UsersIdentityService()
        {
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ConscienceAccount CurrentUser
        {
            get
            {
                if (HttpContext.Current.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                {
                    var userId = HttpContext.Current.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
                    return UserManager.Users.FirstOrDefault(u => u.Id == userId);
                }

                return null;
            }
        }

        public async Task ChangePasswordAsync(int userId, string newPassword)
        {
            //TODO: Check if we need an admin method
            await UserManager.ChangePasswordAsync(userId, string.Empty, newPassword);
        }

        public async Task<ConscienceAccount> LoginAsync(string username, string password)
        {
            var result = await SignInManager.PasswordSignInAsync(username, password, true, shouldLockout: false);

            if (result != SignInStatus.Success)
                throw new ArgumentException(result.ToString());
            
            var user = UserManager.Users.FirstOrDefault(u => u.UserName == username);
            
            return user;
        }

        public async Task LogoffAsync()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        public async Task<ConscienceAccount> RegisterAsync(string username, string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                email = username + "@conscience.com";

            var user = new ConscienceAccount { UserName = username, Email = email };

            var result = await UserManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return await LoginAsync(username, password);
            }
            else
                throw new Exception(result.Errors.FirstOrDefault());
        }
    }
}