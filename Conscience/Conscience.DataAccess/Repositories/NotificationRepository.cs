using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Concience.DataAccess.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>
    {
        public NotificationRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<Notification> DbSet
        {
            get
            {
                return _context.Notifications;
            }
        }
    }
}
