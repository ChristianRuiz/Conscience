using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class User
    {
        public Guid Id
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
        
        public Device Device
        {
            get;
            set;
        }

        public Host Host
        {
            get;
            set;
        }

        public List<Notification> Notifications
        {
            get;
            set;
        }
    }
}
