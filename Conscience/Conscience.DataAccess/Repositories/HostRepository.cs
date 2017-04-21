using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Conscience.DataAccess.Repositories
{
    public class HostRepository : BaseRepository<Host>
    {
        public HostRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<Host> DbSet
        {
            get
            {
                return _context.Hosts;
            }
        }

        public IQueryable<Host> GetAllHosts(Account currentUser)
        {
            var hosts = GetAll().OfType<Host>();
            if (!currentUser.Roles.Any(r => r.Name == RoleTypes.Admin.ToString()
                                        || r.Name == RoleTypes.CompanyAdmin.ToString()))
                hosts = hosts.Where(h => !h.Hidden);
            return hosts;
        }
        
        public Host AddHost(int accountId)
        {
            var account = _context.Accounts.First(a => a.Id == accountId);
            var employee = new Host
            {
                Account = account
            };
            DbSet.Add(employee);
            _context.SaveChanges();
            return employee;
        }
        
        public Host GetById(int userId)
        {
            var host = DbSet.Include(u => u.Account).FirstOrDefault(u => u.Id == userId);
            if (host == null)
                throw new ArgumentException("There is no host with id: " + userId);
            return host;
        }

        public Host GetByAccountId(int accountId)
        {
            return DbSet.FirstOrDefault(u => u.Account.Id == accountId);
        }
    }
}
