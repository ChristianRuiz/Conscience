using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Conscience.Web.Models;
using Conscience.Domain;
using Concience.DataAccess;
using Microsoft.Practices.Unity;

namespace Conscience.Web
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class ApplicationUserStore : IUserStore<ConscienceIdentityUser, int>, IUserPasswordStore<ConscienceIdentityUser, int>, IUserEmailStore<ConscienceIdentityUser, int>, IUserLockoutStore<ConscienceIdentityUser, int>, IUserTwoFactorStore<ConscienceIdentityUser, int>
    {
        private readonly ConscienceContext _context;
        public ApplicationUserStore(ConscienceContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ConscienceIdentityUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ConscienceIdentityUser user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
        }
        
        public async Task<ConscienceIdentityUser> FindByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<ConscienceIdentityUser> FindByNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task UpdateAsync(ConscienceIdentityUser user)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetPasswordHashAsync(ConscienceIdentityUser user)
        {
            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(ConscienceIdentityUser user)
        {
            return !string.IsNullOrWhiteSpace(user.PasswordHash);
        }
        
        public async Task SetPasswordHashAsync(ConscienceIdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            await _context.SaveChangesAsync();
        }

        public async Task<ConscienceIdentityUser> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task SetEmailAsync(ConscienceIdentityUser user, string email)
        {
            user.Email = email;
            await _context.SaveChangesAsync();
        }

        public async Task SetEmailConfirmedAsync(ConscienceIdentityUser user, bool confirmed)
        {
        }

        public async Task<string> GetEmailAsync(ConscienceIdentityUser user)
        {
            return user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(ConscienceIdentityUser user)
        {
            return !string.IsNullOrWhiteSpace(user.Email);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ConscienceIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public async Task SetLockoutEndDateAsync(ConscienceIdentityUser user, DateTimeOffset lockoutEnd)
        {
        }

        public async Task<int> IncrementAccessFailedCountAsync(ConscienceIdentityUser user)
        {
            return 1;
        }

        public async Task ResetAccessFailedCountAsync(ConscienceIdentityUser user)
        {
        }

        public async Task<int> GetAccessFailedCountAsync(ConscienceIdentityUser user)
        {
            return 1;
        }

        public async Task<bool> GetLockoutEnabledAsync(ConscienceIdentityUser user)
        {
            return false;
        }

        public async Task SetLockoutEnabledAsync(ConscienceIdentityUser user, bool enabled)
        {
        }

        public async Task SetTwoFactorEnabledAsync(ConscienceIdentityUser user, bool enabled)
        {
        }

        public async Task<bool> GetTwoFactorEnabledAsync(ConscienceIdentityUser user)
        {
            return false;
        }
    }

    // Configure the application ConscienceIdentityUser manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ConscienceIdentityUser, int>
    {
        public ApplicationUserManager(ApplicationUserStore store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var container = UnityConfig.GetConfiguredContainer();
            var manager = container.Resolve<ApplicationUserManager>();
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ConscienceIdentityUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6
            };

            // Configure ConscienceIdentityUser lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the ConscienceIdentityUser
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ConscienceIdentityUser, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ConscienceIdentityUser, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ConscienceIdentityUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ConscienceIdentityUser, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ConscienceIdentityUser ConscienceIdentityUser)
        {
            return ConscienceIdentityUser.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
