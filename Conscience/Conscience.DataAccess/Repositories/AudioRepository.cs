using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Conscience.DataAccess.Repositories
{
    public class AudioRepository : BaseRepository<Audio>
    {
        public AudioRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<Audio> DbSet
        {
            get
            {
                return _context.Audios;
            }
        }
    }
}
