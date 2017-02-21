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

    public class ApplicationUserStore : IUserStore<ConscienceAccount, int>, IUserPasswordStore<ConscienceAccount, int>, IUserEmailStore<ConscienceAccount, int>, IUserLockoutStore<ConscienceAccount, int>, IUserTwoFactorStore<ConscienceAccount, int>
    {
        private readonly ConscienceContext _context;
        public ApplicationUserStore(ConscienceContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ConscienceAccount user)
        {
            _context.Accounts.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ConscienceAccount user)
        {
            _context.Accounts.Remove(user);
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
        }
        
        public async Task<ConscienceAccount> FindByIdAsync(int userId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<ConscienceAccount> FindByNameAsync(string userName)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task UpdateAsync(ConscienceAccount user)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetPasswordHashAsync(ConscienceAccount user)
        {
            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(ConscienceAccount user)
        {
            return !string.IsNullOrWhiteSpace(user.PasswordHash);
        }
        
        public async Task SetPasswordHashAsync(ConscienceAccount user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            await _context.SaveChangesAsync();
        }

        public async Task<ConscienceAccount> FindByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task SetEmailAsync(ConscienceAccount user, string email)
        {
            user.Email = email;
            await _context.SaveChangesAsync();
        }

        public async Task SetEmailConfirmedAsync(ConscienceAccount user, bool confirmed)
        {
        }

        public async Task<string> GetEmailAsync(ConscienceAccount user)
        {
            return user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(ConscienceAccount user)
        {
            return !string.IsNullOrWhiteSpace(user.Email);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ConscienceAccount user)
        {
            throw new NotImplementedException();
        }

        public async Task SetLockoutEndDateAsync(ConscienceAccount user, DateTimeOffset lockoutEnd)
        {
        }

        public async Task<int> IncrementAccessFailedCountAsync(ConscienceAccount user)
        {
            return 1;
        }

        public async Task ResetAccessFailedCountAsync(ConscienceAccount user)
        {
        }

        public async Task<int> GetAccessFailedCountAsync(ConscienceAccount user)
        {
            return 1;
        }

        public async Task<bool> GetLockoutEnabledAsync(ConscienceAccount user)
        {
            return false;
        }

        public async Task SetLockoutEnabledAsync(ConscienceAccount user, bool enabled)
        {
        }

        public async Task SetTwoFactorEnabledAsync(ConscienceAccount user, bool enabled)
        {
        }

        public async Task<bool> GetTwoFactorEnabledAsync(ConscienceAccount user)
        {
            return false;
        }
    }

    // Configure the application ConscienceIdentityUser manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ConscienceAccount, int>
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
            manager.UserValidator = new UserValidator<ConscienceAccount, int>(manager)
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
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ConscienceAccount, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ConscienceAccount, int>
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
                    new DataProtectorTokenProvider<ConscienceAccount, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ConscienceAccount, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ConscienceAccount ConscienceIdentityUser)
        {
            return ConscienceIdentityUser.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
