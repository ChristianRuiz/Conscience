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

        public IQueryable<ConscienceAccount> GetAllHosts(Account currentUser)
        {
            var hosts = GetAll().Where(a => a.Host != null);
            if (!currentUser.Roles.Any(r => r.Name == RoleTypes.Admin.ToString()
                                        || r.Name == RoleTypes.CompanyAdmin.ToString()))
                hosts = hosts.Where(a => !a.Host.Hidden);
            return hosts;
        }

        public IQueryable<ConscienceAccount> GetAllEmployees()
        {
            return GetAll().Where(a => a.Employee != null);
        }
    }
}
