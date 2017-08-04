using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Conscience.DataAccess.Repositories
{
    public class PlotRepository : BaseRepository<Plot>
    {
        public PlotRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<Plot> DbSet
        {
            get
            {
                return _context.Plots;
            }
        }

        public Plot GetById(int id)
        {
            return DbSet.FirstOrDefault(p => p.Id == id);
        }
    }
}
