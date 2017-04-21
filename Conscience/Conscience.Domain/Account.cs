using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Account
    {
        public Account()
        {
            Roles = new HashSet<Role>();
        }

        public int Id
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

        public string PasswordHash
        {
            get;
            set;
        }

        public Host Host
        {
            get;
            set;
        }

        public Employee Employee
        {
            get;
            set;
        }

        public Device Device
        {
            get;
            set;
        }

        public virtual ICollection<Role> Roles
        {
            get;
            set;
        }
    }
}
