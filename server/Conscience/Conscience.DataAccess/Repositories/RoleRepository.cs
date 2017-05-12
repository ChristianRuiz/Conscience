using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Conscience.DataAccess.Repositories
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<Role> DbSet
        {
            get
            {
                return _context.Roles;
            }
        }
    }
}
