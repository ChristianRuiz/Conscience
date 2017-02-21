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

        public User User
        {
            get;
            set;
        }

        public List<Role> Roles
        {
            get;
            set;
        }
    }
}
