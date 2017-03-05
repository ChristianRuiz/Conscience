using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Conscience.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<User> DbSet
        {
            get
            {
                return _context.Users;
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

        public IQueryable<Employee> GetAllEmployees()
        {
            return GetAll().OfType<Employee>();
        }
    }
}
