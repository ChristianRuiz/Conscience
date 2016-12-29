using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class NotificationStatChange : Notification
    {
        public Stats Stat
        {
            get;
            set;
        }
    }
}
