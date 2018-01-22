using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Conscience.DataAccess.Repositories
{
    public class CharacterRepository : BaseRepository<Character>
    {
        public CharacterRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<Character> DbSet
        {
            get
            {
                return _context.Characters;
            }
        }

        public IQueryable<Character> GetAllCharacters(Account currentUser)
        {
            var characters = GetAll();
            if (!currentUser.Roles.Any(r => r.Name == RoleTypes.Admin.ToString()))
                characters = characters.Where(c => !c.Hosts.Any() || !c.Hosts.Any(h => h.UnassignedOn == null && h.Host.Hidden && !h.Host.HiddenHostAdministrators.Any(a => a.Account.Id == currentUser.Id)));
            return characters;
        }

        public Character GetById(int id)
        {
            return DbSet.Include(c => c.Relations).FirstOrDefault(c => c.Id == id);
        }
    }
}
