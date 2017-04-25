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
        
        public Host GetById(Account currentUser, int userId)
        {
            return GetAllHosts(currentUser).FirstOrDefault(u => u.Id == userId);
        }
    }
}
