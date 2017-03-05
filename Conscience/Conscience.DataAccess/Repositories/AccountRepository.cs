using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Conscience.DataAccess.Repositories
{
    public class AccountRepository : BaseRepository<ConscienceAccount>
    {
        private readonly UserRepository _userRepo;

        public AccountRepository(ConscienceContext context, UserRepository userRepo) : base(context)
        {
            _userRepo = userRepo;
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
            return _userRepo.GetAllHosts(currentUser).Select(e => e.Account);
        }

        public IQueryable<Account> GetAllEmployees()
        {
            return _userRepo.GetAllEmployees().Select(e => e.Account);
        }
    }
}
