using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Employee : IdentityEntity
    {
        public Employee()
        {
            Notifications = new HashSet<Notification>();
            HiddenHosts = new HashSet<Host>();
        }
        
        public string Name
        {
            get;
            set;
        }

        public virtual ICollection<Notification> Notifications
        {
            get;
            set;
        }

        public virtual ICollection<Host> HiddenHosts
        {
            get;
            set;
        }

        public virtual Account Account
        {
            get;
            set;
        }
    }
}
