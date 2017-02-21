using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Concience.DataAccess.Repositories
{
    public class DeviceRepository : BaseRepository<Device>
    {
        public DeviceRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<Device> DbSet
        {
            get
            {
                return _context.Devices;
            }
        }
    }
}
