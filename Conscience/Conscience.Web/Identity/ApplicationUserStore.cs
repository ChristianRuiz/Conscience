using Conscience.DataAccess;
using Conscience.DataAccess.Repositories;
using Conscience.Domain;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Conscience.Web.Identity
{
    public class ApplicationUserStore : IUserStore<ConscienceAccount, int>, IUserPasswordStore<ConscienceAccount, int>, IUserEmailStore<ConscienceAccount, int>, IUserLockoutStore<ConscienceAccount, int>, IUserTwoFactorStore<ConscienceAccount, int>, IQueryableUserStore<ConscienceAccount, int>, IUserRoleStore<ConscienceAccount, int>
    {
        private readonly ConscienceContext _context;
        private readonly RoleRepository _rolesRepo;
        private readonly AccountRepository _accountsRepo;

        public IQueryable<ConscienceAccount> Users
        {
            get
            {
                return _context.Accounts;
            }
        }

        public ApplicationUserStore(ConscienceContext context, RoleRepository rolesRepo, AccountRepository accountsRepo)
        {
            _context = context;
            _rolesRepo = rolesRepo;
            _accountsRepo = accountsRepo;
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

        public async Task AddToRoleAsync(ConscienceAccount user, string roleName)
        {
            var role = await _rolesRepo.GetAll().FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());
            if (role == null)
            {
                role = new Role { Name = roleName };
                _rolesRepo.Add(role);
            }

            var userToModify = _accountsRepo.GetAll().First(a => a.Id == user.Id);
            userToModify.Roles.Add(role);
            _accountsRepo.Modify(userToModify);
        }

        public async Task RemoveFromRoleAsync(ConscienceAccount user, string roleName)
        {
            var role = await _rolesRepo.GetAll().FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());

            var userToModify = _accountsRepo.GetAll().First(a => a.Id == user.Id);
            userToModify.Roles.Remove(role);
            _accountsRepo.Modify(userToModify);
        }

        public async Task<IList<string>> GetRolesAsync(ConscienceAccount user)
        {
            return await _rolesRepo.GetAll().Select(r => r.Name).ToListAsync();
        }

        public async Task<bool> IsInRoleAsync(ConscienceAccount user, string roleName)
        {
            return user.Roles.Any(r => r.Name.ToLowerInvariant() == roleName.ToLowerInvariant());
        }
    }
}