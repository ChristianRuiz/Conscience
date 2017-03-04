using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Concience.DataAccess.Repositories
{
    public class AccountRepository : BaseRepository<ConscienceAccount>
    {
        public AccountRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<ConscienceAccount> DbSet
        {
            get
            {
                return _context.Accounts;
            }
        }

        public IQueryable<Account> GetAllHosts(Account currentUser)
        {
            var hosts = _context.Users.OfType<Host>().Select(e => e.Account);
            if (!currentUser.Roles.Any(r => r.Name == RoleTypes.Admin.ToString()
                                        || r.Name == RoleTypes.CompanyAdmin.ToString()))
                hosts = hosts.Where(a => !a.Host.Hidden);
            return hosts;
        }

        public IQueryable<Account> GetAllEmployees()
        {
            return _context.Users.OfType<Employee>().Select(e => e.Account);
        }
    }
}
