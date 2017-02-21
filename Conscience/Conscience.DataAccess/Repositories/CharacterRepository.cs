using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Concience.DataAccess.Repositories
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
    }
}
