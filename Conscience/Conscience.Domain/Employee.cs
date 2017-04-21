using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Employee
    {
        public Employee()
        {
            Notifications = new HashSet<Notification>();
        }

        public int Id
        {
            get;
            set;
        }

        public virtual ICollection<Notification> Notifications
        {
            get;
            set;
        }

        public Account Account
        {
            get;
            set;
        }
    }
}
