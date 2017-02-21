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
    }
}
