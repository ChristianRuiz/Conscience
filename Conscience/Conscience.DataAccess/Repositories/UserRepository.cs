using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Concience.DataAccess.Repositories
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
            return GetAll().OfType<Host>();
        }

        public IQueryable<Employee> GetAllEmployees()
        {
            return GetAll().OfType<Employee>();
        }
    }
}
