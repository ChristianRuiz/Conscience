using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Concience.DataAccess.Repositories
{
    public class MemoryRepository : BaseRepository<Memory>
    {
        public MemoryRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<Memory> DbSet
        {
            get
            {
                return _context.Memories;
            }
        }

        public IQueryable<CoreMemory> GetAllCoreMemories()
        {
            return GetAll().OfType<CoreMemory>();
        }
    }
}
